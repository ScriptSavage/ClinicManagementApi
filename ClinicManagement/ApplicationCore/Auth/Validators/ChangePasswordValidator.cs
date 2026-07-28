using ApplicationCore.Auth.Dto;
using FluentValidation;

namespace ApplicationCore.Auth.Validators;

public class ChangePasswordValidator : AbstractValidator<AuthDto.ChangePasswordDto>
{
    public ChangePasswordValidator()
    {
        RuleFor(e=>e.CurrentPassword)
            .NotEmpty()
            .WithMessage("Current password is required")
            .NotNull()
            .WithMessage("Current password is required");
        
        RuleFor(e => e.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required")
            .NotNull()
            .WithMessage("New password is required")
            .MaximumLength(8)
            .WithMessage("New password must not exceed 8 characters");
        
        RuleFor(e=>e.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Confirm password is required")
            .NotNull()
            .WithMessage("Confirm password is required")
            .MaximumLength(8)
            .WithMessage("Confirm password must not exceed 8 characters")
            .Equal(e => e.NewPassword)
            .WithMessage("Passwords must match");
    }
}