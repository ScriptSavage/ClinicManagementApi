using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApplicationCore.Address.Dto;
using ApplicationCore.Auth.Dto;
using ApplicationCore.Exceptions;
using FluentValidation;
using Infrastructure.Entities;
using Infrastructure.Helpers;
using Infrastructure.Repositories.Address;
using Infrastructure.Repositories.Auth;
using Infrastructure.Repositories.Patient;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ApplicationCore.Auth.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IPatientRepository  _patientRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IValidator<AuthDto.RegisterNewPatient> _registerNewPatientValidator;
    private readonly IValidator<AuthDto.LoginDto> _loginValidator;
    private readonly IValidator<AddressDto.NewAddress> _registerNewAddressValidator;
    private readonly IValidator<AuthDto.ChangePasswordDto>  _changePasswordValidator;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository;


    public AuthService(UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        IPatientRepository patientRepository,
        IAddressRepository addressRepository,
        IValidator<AuthDto.RegisterNewPatient> registerNewPatientValidator,
        IValidator<AddressDto.NewAddress> registerNewAddressValidator,
        IValidator<AuthDto.LoginDto> loginValidator,
        IValidator<AuthDto.ChangePasswordDto> changePasswordValidator,
        IConfiguration configuration,
        SignInManager<ApplicationUser> signInManager,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _patientRepository = patientRepository;
        _addressRepository = addressRepository;
        _registerNewPatientValidator = registerNewPatientValidator;
        _registerNewAddressValidator = registerNewAddressValidator;
        _loginValidator = loginValidator;
        _changePasswordValidator = changePasswordValidator;
        _configuration = configuration;
        _signInManager = signInManager;
        _refreshTokenRepository = refreshTokenRepository;
    }
    
    private const string RoleClaimType = "role";


    public async Task<string> RegisterNewPatient(AuthDto.RegisterNewPatient request)
    {
        var validationResult = await _registerNewPatientValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        if (request.Password != request.ConfirmPassword)
        {
            throw new ValidationException("Passwords do not match");
        }

        await using var transaction = await _unitOfWork.BeginTransactionAsync();

        string login = await GenerateUniqueLoginAsync();
        
        try
        {
            var newUserAccount = new ApplicationUser
            {
                Email = request.EmailAddress,
                UserName = login,
                PhoneNumber = request.PhoneNumber
            };

            var createUserResult = await _userManager.CreateAsync(newUserAccount, request.Password);

            if (!createUserResult.Succeeded)
            {
                var errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));

                throw new InvalidOperationException($" Cannot create user {errors}");
            }
            
            await _userManager.AddToRoleAsync(newUserAccount, "Patient");
            

            var newPatient = new Infrastructure.Entities.Patient
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Pesel = request.Pesel,
                DateOfBirth = request.DateOfBirth,
                UserId = newUserAccount.Id
            };

            var newAddressValidationResult = await _registerNewAddressValidator
                .ValidateAsync(request.Address);

            if (!newAddressValidationResult.IsValid)
            {
                throw new ValidationException(newAddressValidationResult.Errors);
            }

            newPatient.Address = new Infrastructure.Entities.Address()
            {
                Street = request.Address.Street,
                City = request.Address.City,
                PostalCode = request.Address.ZipCode
            };
            await _addressRepository.CreateAsync(newPatient.Address);
            await _patientRepository.AddNewPatientAsync(newPatient);
            await _unitOfWork.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return login;
    }


    
    public async Task<AuthDto.AuthResponse> LoginAsync(AuthDto.LoginDto request)
    {
        var validationResult = await _loginValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var user = await _userManager.FindByNameAsync(request.Username);
        if (user is null)
        {
            throw new AuthenticationFailedException("User or password is incorrect");
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        if (!signInResult.Succeeded)
        {
            throw new AuthenticationFailedException("User or password is incorrect");
        }

        var accessToken = await GenerateAccessTokenAsync(user);
        var refreshToken = GenerateRefreshToken();
        var now = DateTime.UtcNow;
        var storedToken = CreateRefreshToken(user, refreshToken, Guid.NewGuid(), now,
            now.AddDays(GetPositiveJwtSetting("RefreshTokenExpirationDays")));

        await _refreshTokenRepository.AddAsync(storedToken);
        await _unitOfWork.SaveChangesAsync();

        return new AuthDto.AuthResponse(accessToken, refreshToken);
    }

    public async Task<AuthDto.AuthResponse> RefreshAsync(AuthDto.RefreshTokenDto request)
    {
        ValidateRefreshToken(request.RefreshToken);
        var storedToken = await _refreshTokenRepository.FindByHashAsync(HashToken(request.RefreshToken));
        if (storedToken is null || storedToken.Expires <= DateTime.UtcNow)
        {
            throw new AuthenticationFailedException("Refresh token is invalid or expired");
        }

        var user = await _userManager.FindByIdAsync(storedToken.UserId.ToString());
        if (user is null || await _userManager.IsLockedOutAsync(user) ||
            storedToken.SecurityStamp != user.SecurityStamp)
        {
            await _refreshTokenRepository.RevokeFamilyAsync(storedToken.FamilyId, DateTime.UtcNow);
            throw new AuthenticationFailedException("Refresh token is invalid or expired");
        }

        var accessToken = await GenerateAccessTokenAsync(user);
        var refreshToken = GenerateRefreshToken();
        var now = DateTime.UtcNow;
        var replacement = CreateRefreshToken(user, refreshToken, storedToken.FamilyId, now, storedToken.Expires);

        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        if (!await _refreshTokenRepository.TryRevokeAsync(storedToken.RefreshTokenId, replacement.TokenHash, now))
        {
            await _refreshTokenRepository.RevokeFamilyAsync(storedToken.FamilyId, now);
            await transaction.CommitAsync();
            throw new AuthenticationFailedException("Refresh token is invalid or expired");
        }

        await _refreshTokenRepository.AddAsync(replacement);
        await _unitOfWork.SaveChangesAsync();
        await transaction.CommitAsync();

        return new AuthDto.AuthResponse(accessToken, refreshToken);
    }

    public async Task LogoutAsync(AuthDto.RefreshTokenDto request)
    {
        ValidateRefreshToken(request.RefreshToken);
        var storedToken = await _refreshTokenRepository.FindByHashAsync(HashToken(request.RefreshToken));
        if (storedToken is not null)
        {
            await _refreshTokenRepository.RevokeFamilyAsync(storedToken.FamilyId, DateTime.UtcNow);
        }
    }

    
    public async Task ChangePasswordAsync(string userId, AuthDto.ChangePasswordDto request)
    {
        var passwordValidationResult = await _changePasswordValidator.ValidateAsync(request);

        if (!passwordValidationResult.IsValid)
        {
            throw new ValidationException(passwordValidationResult.Errors);
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            throw new ArgumentException("User or password is incorrect");
        }

        if (request.NewPassword != request.ConfirmPassword)
        {
            throw new ArgumentException("Passwords do not match");
        }

        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            throw new ValidationException(string.Join(", ", result.Errors.Select(x => x.Description)));
        }

        await _refreshTokenRepository.RevokeUserTokensAsync(user.Id, DateTime.UtcNow);
        await transaction.CommitAsync();
    }

    public async Task<AuthDto.DetailsDto> GetDetailsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            throw new ArgumentException("User or password is incorrect");
        }
        
        var patient = await _patientRepository.GetPatientByUserIdAsync(user.Id);

        var patientDetails = await _patientRepository.GetPatientByIdAsync(patient.PatientId);
        
       return new AuthDto.DetailsDto(
           patientDetails.FirstName, 
           patientDetails.LastName, 
           patientDetails.Pesel,
           patientDetails.DateOfBirth,
           user.Email,
           user.PhoneNumber,
           new AddressDto.NewAddress(
               patientDetails.Address.Street,
               patientDetails.Address.City,
               patientDetails.Address.PostalCode));
    }


    private async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
    {
        var jwtConfig = _configuration.GetRequiredSection("JwtSettings");
        var secret = jwtConfig["Secret"] ?? throw new InvalidOperationException("JWT secret is missing");
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(roles.Select(role => new Claim(RoleClaimType, role)));

        var token = new JwtSecurityToken(
            issuer: jwtConfig["Issuer"],
            audience: jwtConfig["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(GetPositiveJwtSetting("ExpirationMinutes")),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private int GetPositiveJwtSetting(string name)
    {
        if (!int.TryParse(_configuration[$"JwtSettings:{name}"], out var value) || value <= 0)
        {
            throw new InvalidOperationException($"JwtSettings:{name} must be a positive integer");
        }

        return value;
    }

    private static RefreshToken CreateRefreshToken(ApplicationUser user, string token, Guid familyId,
        DateTime created, DateTime expires) => new()
    {
        RefreshTokenId = Guid.NewGuid(),
        UserId = user.Id,
        FamilyId = familyId,
        TokenHash = HashToken(token),
        SecurityStamp = user.SecurityStamp ?? throw new InvalidOperationException("User security stamp is missing"),
        Created = created,
        Expires = expires
    };

    private static void ValidateRefreshToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token) || token.Length != 88)
        {
            throw new ValidationException("A refresh token containing 88 characters is required");
        }
    }

    private static string HashToken(string token) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

    private static string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }

    private async Task<string> GenerateUniqueLoginAsync()
        {
            string login;
            bool loginExists;

            do
            {
                login = GenerateRandomLogin();
                var existingUser = await _userManager.FindByNameAsync(login);
                loginExists = existingUser != null;
            } 
            while (loginExists);

            return login;
        }

        private static string GenerateRandomLogin()
        {
            const int loginLength = 10;
            const string chars = "0123456789";

            var characters = new char[loginLength];
            var randomBuffer = new byte[loginLength];
            
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBuffer);
            }

            for (var i = 0; i < loginLength; i++)
            {
                characters[i] = chars[randomBuffer[i] % chars.Length];
            }

            return  new string(characters);
        }
    }
