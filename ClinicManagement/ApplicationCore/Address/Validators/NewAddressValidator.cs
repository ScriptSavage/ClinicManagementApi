using ApplicationCore.Address.Dto;
using FluentValidation;

namespace ApplicationCore.Address.Validators;

public class NewAddressValidator : AbstractValidator<AddressDto.NewAddress>
{
    public NewAddressValidator()
    {
        RuleFor(p => p.Street)
            .NotEmpty()
            .WithMessage("Street is required")
            .NotNull()
            .WithMessage("Street is required")
            .MaximumLength(150)
            .WithMessage("Street length is 200");


        RuleFor(p => p.City)
            .NotEmpty()
            .WithMessage("City is required")
            .NotNull()
            .WithMessage("City is required")
            .MaximumLength(150);
        
        RuleFor(p => p.ZipCode)
            .NotEmpty()
            .WithMessage("ZipCode is required")
            .NotNull()
            .WithMessage("ZipCode is required")
            .MaximumLength(6);
    }
}