using MediatR;

namespace EasyStay.Application.MediatR.Accounts.Commands.SendResetPasswordEmail;

public class SendResetPasswordEmailCommand : IRequest {
	public string Email { get; set; } = null!;
}
