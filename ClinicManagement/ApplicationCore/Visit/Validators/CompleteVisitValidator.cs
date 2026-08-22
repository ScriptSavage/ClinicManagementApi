using ApplicationCore.Visit.Dto;
using FluentValidation;

namespace ApplicationCore.Visit.Validators;

public class CompleteVisitValidator : AbstractValidator<VisitDto.CreateDescription>
{
    public CompleteVisitValidator()
    {
        RuleFor(e => e.Description)
            .NotEmpty().WithMessage("Description is required")
            .NotNull().WithMessage("Description is required")
            .MinimumLength(5).WithMessage("Description must be at least 5 characters long");
    }
}