using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.Models.Accounts;
using EasyStay.Domain.Constants;
using EasyStay.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace EasyStay.Infrastructure.Services;

public class AuthService(
	IEasyStayDbContext context,
	UserManager<User> userManager,
	IConfiguration configuration,
	IImageService imageService
) : IAuthService {

	public async Task<User> CreateUserAsync(UserDto userDto, CreateUserType type, CancellationToken cancellationToken = default) {
		var user = await CreateAndInicializeUserInstanceAsync(userDto, type);

		try {
			await CreateUserAsync(user, type, userDto.Password, cancellationToken);
		}
		catch {
			imageService.DeleteImageIfExists(user.Photo);
			throw;
		}

		return user;
	}

	private async Task<User> CreateAndInicializeUserInstanceAsync(UserDto userDto, CreateUserType type) {
		string defaultBase64Image = configuration.GetValue<string>("DefaultUserImageBase64")
			?? throw new Exception("DefaultUserImageBase64 is not inicialized");

		var user = CreateUserInstanceByCreateUserType(type);

		user.FirstName = userDto.FirstName;
		user.LastName = userDto.LastName;
		user.Email = userDto.Email;
		user.UserName = userDto.UserName;
		user.Photo = await (userDto.Image is not null
			? imageService.SaveImageAsync(userDto.Image)
			: imageService.SaveImageAsync(defaultBase64Image));

		return user;
	}

	private async Task CreateUserAsync(User user, CreateUserType type, string? password = null, CancellationToken cancellationToken = default) {
		using var transaction = await context.BeginTransactionAsync(cancellationToken);

		try {
			IdentityResult identityResult = await CreateUserInDatabaseAsync(user, password);
			if (!identityResult.Succeeded)
				throw new IdentityException(identityResult, "User creating error");

			identityResult = await userManager.AddToRoleAsync(user, GetRoleByCreateUserType(type));
			if (!identityResult.Succeeded)
				throw new IdentityException(identityResult, "Role assignment error");

			await transaction.CommitAsync(cancellationToken);
		}
		catch {
			await transaction.RollbackAsync(cancellationToken);
			throw;
		}
	}

	private static User CreateUserInstanceByCreateUserType(CreateUserType type) =>
		type switch {
			CreateUserType.Customer => new Customer(),
			CreateUserType.Realtor => new Realtor(),
			CreateUserType.Admin => new Admin(),
			_ => throw new Exception("Invalid type"),
		};

	private static string GetRoleByCreateUserType(CreateUserType type) =>
		type switch {
			CreateUserType.Customer => Roles.Customer,
			CreateUserType.Realtor => Roles.Realtor,
			CreateUserType.Admin => Roles.Admin,
			_ => throw new Exception("Invalid type")
		};

	private Task<IdentityResult> CreateUserInDatabaseAsync(User user, string? password) =>
		string.IsNullOrEmpty(password)
			? userManager.CreateAsync(user)
			: userManager.CreateAsync(user, password);
}
