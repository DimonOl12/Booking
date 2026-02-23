using EasyStay.Application.Interfaces;
using EasyStay.Domain.Constants;
using FluentValidation;

namespace EasyStay.Application.MediatR.Accounts.Commands.Registration;

public class RegistrationValidator : AbstractValidator<RegistrationCommand> {
	public RegistrationValidator(IIdentityValidator identityValidator, IImageValidator imageValidator) {
		RuleFor(r => r.Email)
			.NotEmpty()
				.WithMessage("Email is empty or null")
			.MaximumLength(100)
				.WithMessage("Email is too long")
			.EmailAddress()
				.WithMessage("Email is invalid")
			.MustAsync(identityValidator.IsNewEmailAsync)
				.WithMessage("There is already a user with this email");

		RuleFor(r => r.UserName)
			.NotEmpty()
				.WithMessage("Username is empty or null")
			.MaximumLength(100)
				.WithMessage("Username is too long")
			.MustAsync(identityValidator.IsNewUserNameAsync)
				.WithMessage("There is already a user with this username");

		RuleFor(r => r.FirstName)
			.NotEmpty()
				.WithMessage("FirstName is empty or null")
			.MaximumLength(100)
				.WithMessage("FirstName is too long");

		RuleFor(r => r.LastName)
			.NotEmpty()
				.WithMessage("LastName is empty or null")
			.MaximumLength(100)
				.WithMessage("LastName is too long");

		RuleFor(r => r.Image)
			.MustAsync(imageValidator.IsValidNullPossibeImageAsync)
				.WithMessage("Image is not valid");

		RuleFor(r => r.Password)
			.NotEmpty()
				.WithMessage("Password is empty or null")
			.MinimumLength(8)
				.WithMessage("Password is too short");

		RuleFor(r => r.Type)
			.Must(t => Enum.TryParse(t, out RegistrationUserType _))
				.WithMessage(BuildTypeError());
	}

	private static string BuildTypeError() {
		var validTypes = string.Join(", ", Enum.GetNames(typeof(RegistrationUserType)));

		return $"Type is not valid. Valid types: [ {validTypes} ]";
	}
}
