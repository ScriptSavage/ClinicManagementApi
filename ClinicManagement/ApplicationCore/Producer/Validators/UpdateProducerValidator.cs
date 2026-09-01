using ApplicationCore.Producer.Dto;
using FluentValidation;

namespace ApplicationCore.Producer.Validators;

public class UpdateProducerValidator : AbstractValidator<ProducerDto.UpdateProducer>
{
    public UpdateProducerValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(2)
            .WithMessage("Minimum length of Name is 2")
            .MaximumLength(250)
            .WithMessage("Maximum length of Name is 250");
    }
}