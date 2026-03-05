using EasyStay.Application.Interfaces;
using FluentValidation;

namespace EasyStay.Application.MediatR.Accounts.Commands.UnlockUserById;

public class UnlockUserByIdValidator : AbstractValidator<UnlockUserByIdCommand> {
	public UnlockUserByIdValidator(ICurrentUserService currentUserService) {
		RuleFor(u => u.Id)
			.NotEqual(currentUserService.GetRequiredUserId())
				.WithMessage("You cannot unlock yourself.");
	}
}
