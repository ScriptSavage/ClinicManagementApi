using ApplicationCore.Producer.Dto;
using FluentValidation;

namespace ApplicationCore.Producer.Validators;

public class NewProducerValidator :  AbstractValidator<ProducerDto.NewProducer>
{
    public NewProducerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .NotNull().WithMessage("Name is required")
            .MinimumLength(2);
    }
}