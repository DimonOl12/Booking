using Reservio.Application.Interfaces;
using FluentValidation;

namespace Reservio.Application.MediatR.Accounts.Commands.UpdateUserInfo;

public class UpdateUserInfoValidator : AbstractValidator<UpdateUserInfoCommand> {
	public UpdateUserInfoValidator(IIdentityValidator identityValidator, IImageValidator imageValidator) {
		RuleFor(u => u.Email)
			.NotEmpty()
				.WithMessage("Email is empty or null")
			.MaximumLength(100)
				.WithMessage("Email is too long")
			.EmailAddress()
				.WithMessage("Email is invalid")
			.MustAsync(identityValidator.IsNewOrCurrentEmailAsync)
				.WithMessage("There is already a user with this email");

		RuleFor(u => u.FirstName)
			.NotEmpty()
				.WithMessage("FirstName is empty or null")
			.MaximumLength(100)
				.WithMessage("FirstName is too long");

		RuleFor(u => u.LastName)
			.NotEmpty()
				.WithMessage("LastName is empty or null")
			.MaximumLength(100)
				.WithMessage("LastName is too long");

		When(u => u.Photo != null, () => {
			RuleFor(u => u.Photo)
				.MustAsync(imageValidator.IsValidImageAsync!)
					.WithMessage("Image is not valid");
		});
	}
}

