using EasyStay.Application.Interfaces;
using EasyStay.Domain.Constants;
using EasyStay.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EasyStay.Infrastructure.Services;

public class DbInitializer(
	EasyStayDbContext context,
	IConfiguration configuration,
	UserManager<User> userManager,
	RoleManager<Role> roleManager,
	IImageService imageService
) : IDbInicializer {

	public async Task InitializeAsync(CancellationToken cancellationToken = default) {
		await MigrateAsync(cancellationToken);

		await InitializeIdentityAsync(cancellationToken);
	}

	public async Task MigrateAsync(CancellationToken cancellationToken) {
		await context.Database.MigrateAsync(cancellationToken);
	}

	public async Task InitializeIdentityAsync(CancellationToken cancellationToken) {
		using var transaction = await context.BeginTransactionAsync(cancellationToken);

		try {
			if (!await roleManager.Roles.AnyAsync(cancellationToken)) {
				await CreateRolesAsync();
			}

			if (!await userManager.Users.AnyAsync(cancellationToken)) {
				await CreateAdminAsync();
			}

			await transaction.CommitAsync(cancellationToken);
		}
		catch {
			transaction.Rollback();
			throw;
		}
	}

	private async Task CreateRolesAsync() {
		foreach (var roleName in Roles.All) {
			await roleManager.CreateAsync(new Role {
				Name = roleName
			});
		}
	}

	private async Task CreateAdminAsync() {
		string defaultBase64Image = configuration.GetValue<string>("DefaultUserImageBase64")
			?? throw new Exception("DefaultUserImageBase64 is not inicialized");

		var admin = new Admin {
			FirstName = "Олег",
			LastName = "Ольжич",
			Email = configuration["Admin:Email"]
				?? throw new NullReferenceException("Admin:Email"),
			UserName = "admin",
			Photo = await imageService.SaveImageAsync(defaultBase64Image)
		};

		IdentityResult result = await userManager.CreateAsync(
			admin,
			configuration["Admin:Password"]
				?? throw new NullReferenceException("Admin:Password")
		);

		if (!result.Succeeded)
			throw new Exception("Error creating admin account");

		result = await userManager.AddToRoleAsync(admin, Roles.Admin);

		if (!result.Succeeded)
			throw new Exception("Role assignment error");
	}
}
