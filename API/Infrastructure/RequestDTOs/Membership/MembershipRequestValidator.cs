using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Membership;

public class MembershipRequestValidator : AbstractValidator<MembershipRequest>
{
    public MembershipRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.DurationDays)
            .GreaterThan(0).WithMessage("Duration in days must be greater than 0.");
    }
}
