using EasyStay.Application.MediatR.Accounts.Commands.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.Accounts.Commands.SignIn;

public class SignInCommand : IRequest<JwtTokenVm> {
	public string Email { get; set; } = null!;
	public string Password { get; set; } = null!;
}
