using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Sport;

public class SportRequestValidator : AbstractValidator<SportRequest>
{
    public SportRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MinimumLength(10).WithMessage("Description must be at least 10 characters long.")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}
