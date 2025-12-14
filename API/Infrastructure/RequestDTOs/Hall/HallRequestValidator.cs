using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Hall;

public class HallRequestValidator : AbstractValidator<HallRequest>
{
    public HallRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Capacity must be greater than 0.");

        RuleFor(x => x.Floor)
            .GreaterThanOrEqualTo(0).WithMessage("Floor must be 0 or higher.");
    }
}
