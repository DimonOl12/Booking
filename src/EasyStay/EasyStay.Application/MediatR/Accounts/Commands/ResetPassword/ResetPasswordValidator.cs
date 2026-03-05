using FluentValidation;

namespace EasyStay.Application.MediatR.Accounts.Commands.ResetPassword;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand> {
	public ResetPasswordValidator() {
		RuleFor(r => r.Email)
			.NotEmpty()
				.WithMessage("Email is empty or null")
			.MaximumLength(100)
				.WithMessage("Email is too long")
			.EmailAddress()
				.WithMessage("Email is invalid");

		RuleFor(r => r.NewPassword)
			.NotEmpty()
				.WithMessage("Password is empty or null")
			.MinimumLength(8)
				.WithMessage("Password is too short");
	}
}
