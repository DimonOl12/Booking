using MediatR;

namespace EasyStay.Application.MediatR.Accounts.Commands.ResetPassword;

public class ResetPasswordCommand : IRequest {
	public string Email { get; set; } = null!;
	public string Token { get; set; } = null!;
	public string NewPassword { get; set; } = null!;
}
