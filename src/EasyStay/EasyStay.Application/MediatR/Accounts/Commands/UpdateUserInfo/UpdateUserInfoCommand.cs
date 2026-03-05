using EasyStay.Application.MediatR.Accounts.Commands.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EasyStay.Application.MediatR.Accounts.Commands.UpdateUserInfo;

public class UpdateUserInfoCommand : IRequest<JwtTokenVm> {
	public string Email { get; set; } = null!;

	public string FirstName { get; set; } = null!;

	public string LastName { get; set; } = null!;

	public IFormFile Photo { get; set; } = null!;
}
