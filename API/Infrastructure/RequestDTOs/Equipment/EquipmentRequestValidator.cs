using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Equipment;

public class EquipmentRequestValidator : AbstractValidator<EquipmentRequest>
{
    public EquipmentRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.QunatityAvailable)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}
