using ApplicationCore.Doctor.Dto;
using FluentValidation;

namespace ApplicationCore.Doctor.Validators;

public class CreateNewDoctorValidator : AbstractValidator<DoctorDto.CreateDoctorDto>
{
    public CreateNewDoctorValidator()
    {
        RuleFor(e=>e.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .NotNull()
            .WithMessage("First name is required")
            .MaximumLength(250)
            .WithMessage("First name cannot exceed 250 characters");
        
        RuleFor(e=>e.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .NotNull()
            .WithMessage("Last name is required")
            .MaximumLength(250)
            .WithMessage("Last name cannot exceed 250 characters");

        RuleFor(e => e.Pwz)
            .NotEmpty()
            .WithMessage("Pwz is required")
            .NotNull()
            .WithMessage("Pwz is required")
            .MaximumLength(20);

        RuleFor(e => e.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .NotNull()
            .WithMessage("Email is required")
            .EmailAddress();

        RuleFor(e => e.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .NotNull()
            .WithMessage("Password is required")
            .MinimumLength(8);

        RuleFor(e => e.Login)
            .NotEmpty()
            .WithMessage("Login is required")
            .NotNull()
            .WithMessage("Login is required");

        RuleFor(e => e.SpecializationsIds)
            .NotEmpty()
            .WithMessage("At  least one specialization is required");
    }
}