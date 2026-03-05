using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Accounts.Commands.Shared;
using EasyStay.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EasyStay.Application.MediatR.Accounts.Commands.SignIn;

public class SignInCommandHandler(
	UserManager<User> userManager,
	IJwtTokenService jwtTokenService
) : IRequestHandler<SignInCommand, JwtTokenVm> {

	public async Task<JwtTokenVm> Handle(SignInCommand request, CancellationToken cancellationToken) {
		User? user = await userManager.FindByEmailAsync(request.Email);

		if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
			throw new UnauthorizedException("Wrong authentication data");

		if (user.LockoutEnd > DateTime.UtcNow)
			throw new ForbiddenException($"Account is locked until {user.LockoutEnd} UTC");

		return new JwtTokenVm {
			Token = await jwtTokenService.CreateTokenAsync(user)
		};
	}
}
