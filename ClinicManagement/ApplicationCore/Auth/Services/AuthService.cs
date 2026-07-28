using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApplicationCore.Address.Dto;
using ApplicationCore.Auth.Dto;
using FluentValidation;
using Infrastructure.Entities;
using Infrastructure.Helpers;
using Infrastructure.Repositories.Address;
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


    public AuthService(UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        IPatientRepository patientRepository,
        IAddressRepository addressRepository,
        IValidator<AuthDto.RegisterNewPatient> registerNewPatientValidator,
        IValidator<AddressDto.NewAddress> registerNewAddressValidator,
        IValidator<AuthDto.LoginDto> loginValidator,
        IValidator<AuthDto.ChangePasswordDto> changePasswordValidator,
        IConfiguration configuration,
        SignInManager<ApplicationUser> signInManager)
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


    
    public async Task<string> GenerateAccessToken(AuthDto.LoginDto request)
    {

            var loginValidationResultAsync = await _loginValidator.ValidateAsync(request);

            if (!loginValidationResultAsync.IsValid)
            {
                throw new ValidationException(loginValidationResultAsync.Errors);
            }

            var user = await _userManager.FindByNameAsync(request.Username);

            if (user is null)
            {
                throw new ArgumentException("User or password is incorrect");
            }


            var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordIsCorrect)
            {
                throw new UnauthorizedAccessException("User or password is incorrect");
            }

            var jwtConfig = _configuration.GetSection("JwtSettings");

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Secret"]));

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(RoleClaimType, role)));


            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtConfig["Issuer"],
                audience: jwtConfig["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtConfig["ExpirationMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
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

        await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
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