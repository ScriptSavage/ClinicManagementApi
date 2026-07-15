using ApplicationCore.Auth.Dto;
using FluentValidation;

namespace ApplicationCore.Auth.Validators;

public class CreatePatientValidator : AbstractValidator<AuthDto.RegisterNewPatient>
{

    public CreatePatientValidator()
    {
        RuleFor(e=>e.FirstName)
            .NotEmpty().WithMessage("First name cannot be empty")
            .NotNull().WithMessage("First name is required");
        
        RuleFor(e=>e.LastName)
            .NotEmpty().WithMessage("Last name cannot be empty")
            .NotNull().WithMessage("Last name is required");
        
        RuleFor(e=>e.Pesel)
            .NotEmpty().WithMessage("Pesel name cannot be empty")
            .NotNull().WithMessage("Pesel name is required")
            .MaximumLength(11).WithMessage("Pesel length must be 11 characters")
            .MinimumLength(11).WithMessage("Pesel length must be 11 characters");

        RuleFor(e => e.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth cannot be empty")
            .NotNull().WithMessage("Date of birth is required");
        
        
        RuleFor(e=>e.EmailAddress)
            .NotEmpty().WithMessage("Email address cannot be empty")
            .NotNull().WithMessage("Email address is required")
            .EmailAddress().WithMessage("Email address is not valid");


        RuleFor(e => e.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .NotNull().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password length must be 8 or more");
        
        RuleFor(e=>e.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm password cannot be empty")
            .NotNull().WithMessage("Confirm password is required")
            .Equal(x=>x.Password).WithMessage("Confirm password doesn't match");
        
        RuleFor(e => e.PhoneNumber)
            .NotEmpty().WithMessage("Phone number cannot be empty")
            .NotNull().WithMessage("Phone number is required");
        
    }
}