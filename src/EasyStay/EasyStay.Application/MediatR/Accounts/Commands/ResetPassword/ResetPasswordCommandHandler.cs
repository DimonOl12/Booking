using EasyStay.Application.Common.Exceptions;
using EasyStay.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EasyStay.Application.MediatR.Accounts.Commands.ResetPassword;

public class ResetPasswordCommandHandler(
	UserManager<User> userManager
) : IRequestHandler<ResetPasswordCommand> {

	public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken) {
		var user = await userManager.FindByEmailAsync(request.Email);

		var exception = new BadRequestException("Invalid token or email");
		var random = new Random();

		if (user is null) {
			await Task.Delay(random.Next(300), cancellationToken);

			throw exception;
		}

		var resetPasswordResult = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
		if (!resetPasswordResult.Succeeded) {
			await Task.Delay(random.Next(300), cancellationToken);

			throw exception;
		}
	}
}
