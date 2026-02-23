using EasyStay.Application.Interfaces;
using FluentValidation;

namespace EasyStay.Application.MediatR.Accounts.Commands.CreateAdmin;

public class CreateAdminValidator : AbstractValidator<CreateAdminCommand> {
	public CreateAdminValidator(IIdentityValidator identityValidator, IImageValidator imageValidator) {
		RuleFor(a => a.Email)
			.NotEmpty()
				.WithMessage("Email is empty or null")
			.MaximumLength(100)
				.WithMessage("Email is too long")
			.EmailAddress()
				.WithMessage("Email is invalid")
			.MustAsync(identityValidator.IsNewEmailAsync)
				.WithMessage("There is already a user with this email");

		RuleFor(a => a.UserName)
			.NotEmpty()
				.WithMessage("Username is empty or null")
			.MaximumLength(100)
				.WithMessage("Username is too long")
			.MustAsync(identityValidator.IsNewUserNameAsync)
				.WithMessage("There is already a user with this username");

		RuleFor(a => a.FirstName)
			.NotEmpty()
				.WithMessage("FirstName is empty or null")
			.MaximumLength(100)
				.WithMessage("FirstName is too long");

		RuleFor(a => a.LastName)
			.NotEmpty()
				.WithMessage("LastName is empty or null")
			.MaximumLength(100)
				.WithMessage("LastName is too long");

		RuleFor(a => a.Image)
			.MustAsync(imageValidator.IsValidNullPossibeImageAsync)
				.WithMessage("Image is not valid");

		RuleFor(a => a.Password)
			.NotEmpty()
				.WithMessage("Password is empty or null")
			.MinimumLength(8)
				.WithMessage("Password is too short");
	}
}
