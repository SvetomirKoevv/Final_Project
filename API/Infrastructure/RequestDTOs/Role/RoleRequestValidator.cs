using FluentValidation;

namespace API.Infrastructure.RequestDTOs.Role;

public class RoleRequestValidator : AbstractValidator<RoleRequest>
{
    public RoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");
    }
}
