using ApplicationCore.Auth.Dto;
using FluentValidation;

namespace ApplicationCore.Auth.Validators;

public class LoginValidator : AbstractValidator<AuthDto.LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required")
            .NotNull()
            .WithMessage("Username is required");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .NotNull()
            .WithMessage("Password is required");
    }
}