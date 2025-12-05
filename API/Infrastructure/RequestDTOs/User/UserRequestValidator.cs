using FluentValidation;

namespace API.Infrastructure.RequestDTOs;

public class UserRequestValidator : AbstractValidator<UserRequest>
{
	public UserRequestValidator()
	{
		RuleFor(x => x.FirstName)
			.NotEmpty().WithMessage("First name is required.")
			.Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");

		RuleFor(x => x.LastName)
			.NotEmpty().WithMessage("Last name is required.")
			.Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

		RuleFor(x => x.Username)
			.NotEmpty().WithMessage("Username is required.")
			.Length(3, 20).WithMessage("Username must be between 3 and 20 characters.")
			.Matches("^[a-zA-Z0-9._-]+$").WithMessage("Username contains invalid characters.");

		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("Invalid email address.");

		RuleFor(x => x.Phone)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage("Phone number is required.")
			.Matches(@"^\+?[0-9]{7,15}$").WithMessage("Phone number must be between 7 and 15 digits and may start with +.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
	}
}
