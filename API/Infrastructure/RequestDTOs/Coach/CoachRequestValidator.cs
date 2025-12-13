using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Coach;

public class CoachRequestValidator : AbstractValidator<CoachRequest>
{
    public CoachRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[0-9]{7,15}$").WithMessage("Phone number must be between 7 and 15 digits and may start with +.");
    }
}
