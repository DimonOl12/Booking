using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Accounts.Commands.Shared;
using EasyStay.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EasyStay.Application.MediatR.Accounts.Commands.UpdateUserInfo;

public class UpdateUserInfoCommandHandler(
	UserManager<User> userManager,
	IJwtTokenService jwtTokenService,
	ICurrentUserService currentUserService,
	IImageService imageService
) : IRequestHandler<UpdateUserInfoCommand, JwtTokenVm> {

	public async Task<JwtTokenVm> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken) {
		var user = await userManager.FindByIdAsync(currentUserService.GetRequiredUserId().ToString())
			?? throw new Exception("User not found");

		user.FirstName = request.FirstName;
		user.LastName = request.LastName;

		var oldPhoto = user.Photo;
		user.Photo = await imageService.SaveImageAsync(request.Photo);

		var identityResult = await userManager.UpdateAsync(user);
		if (identityResult.Succeeded) {
			imageService.DeleteImageIfExists(oldPhoto);
		}
		else {
			imageService.DeleteImageIfExists(user.Photo);
			throw new IdentityException(identityResult);
		}

		identityResult = await userManager.SetEmailAsync(user, request.Email);
		if (!identityResult.Succeeded)
			throw new IdentityException(identityResult);

		return new JwtTokenVm {
			Token = await jwtTokenService.CreateTokenAsync(user),
		};
	}
}
