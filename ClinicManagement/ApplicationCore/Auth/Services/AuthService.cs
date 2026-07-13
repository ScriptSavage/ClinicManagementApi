using System.Security.Cryptography;
using ApplicationCore.Auth.Dto;
using FluentValidation;
using Infrastructure.Entities;
using Infrastructure.Helpers;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Auth.Services;

public class AuthService : IAuthService
{
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPatientRepository  _patientRepository;
    private readonly IValidator<AuthDto.RegisterNewPatient> _registerNewPatientValidator;


    public AuthService(UserManager<ApplicationUser> userManager,
        IPasswordHasher<ApplicationUser> passwordHasher, 
        IUnitOfWork unitOfWork,
        IPatientRepository patientRepository,
        IValidator<AuthDto.RegisterNewPatient> registerNewPatientValidator)
    {
        _userManager = userManager;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _patientRepository = patientRepository;
        _registerNewPatientValidator = registerNewPatientValidator;
    }

    public async Task RegisterNewPatient(AuthDto.RegisterNewPatient request)
    {


       var validationResult = await _registerNewPatientValidator.ValidateAsync(request);

       if (!validationResult.IsValid)
       {
           throw new ValidationException(validationResult.Errors);
       }

       var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var newUserAccount = new ApplicationUser()
            {
                Email = request.EmailAddress,
                UserName = request.EmailAddress,
                PhoneNumber = request.PhoneNumber
            };

            var newPatient = new Patient()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Pesel = request.Pesel,
                DateOfBirth = request.DateOfBirth,
            };

            newPatient.ApplicationUser = newUserAccount;


            if (request.Password != request.ConfirmPassword)
            {
                throw new Exception("Passwords do not match.");
            }

            await _userManager.CreateAsync(newUserAccount, request.Password);
             await _userManager.AddToRoleAsync(newUserAccount, "Patient");
             await _patientRepository.AddNewPatientAsync(newPatient);

            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
           await transaction.RollbackAsync();
           throw;
        }
    }


    private string GenerateLogin()
    {
        const int LoginLength = 10;
        
        var charArray = new char[LoginLength];

        for (var i = 0; i < LoginLength; i++)
        {
            var digit = RandomNumberGenerator.GetInt32(0, 10);
            charArray[i] = (char)('a' + digit);
        }
        
        return new string(charArray);
    }

}