using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities.Identity;
using EasyStay.Infrastructure.Options;
using EasyStay.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EasyStay.Infrastructure;

public static class DependencyInjection {
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
		var connectionString = configuration.GetConnectionString("Npgsql");

		services.Configure<GmailSmtpOptions>(configuration.GetRequiredSection("GmailSmtp"));

		services.AddDbContext<EasyStayDbContext>(options => {
			options.UseNpgsql(connectionString);
		});

		services.AddScoped<IEasyStayDbContext, EasyStayDbContext>();
		services.AddSingleton<IImageService, ImageService>();
		services.AddSingleton<IEmailService, GmailEmailService>();
		services.AddTransient<IDbInicializer, DbInitializer>();
		services.AddTransient<IScopeCoveredDbInicializer, ScopeCoveredDbInicializer>();
		services.AddTransient<ICleanDataSeeder, CleanDataSeeder>();
		services.AddTransient<IGeneratedDataSeeder, GeneratedDataSeeder>();
		services.AddTransient<IAggregateSeeder, AggregateSeeder>();
		services.AddScoped<IAuthService, AuthService>();

		services
			.AddIdentity<User, Role>(options => {
				options.Stores.MaxLengthForKeys = 128;

				options.Password.RequiredLength = 8;
				options.Password.RequireDigit = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;
			})
			.AddEntityFrameworkStores<EasyStayDbContext>()
			.AddDefaultTokenProviders();

		var singinKey = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(
				configuration["Authentication:Jwt:SecretKey"]
					?? throw new NullReferenceException("Authentication:Jwt:SecretKey")
			)
		);

		services
			.AddAuthentication(options => {
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options => {
				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters() {
					ValidateIssuer = false,
					ValidateAudience = false,
					IssuerSigningKey = singinKey,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ClockSkew = TimeSpan.Zero
				};

				options.Events = new JwtBearerEvents {
					OnTokenValidated = async context => {
						if (context.Principal is null)
							return;

						var userManager = context.HttpContext
							.RequestServices
							.GetRequiredService<UserManager<User>>();
						var user = await userManager.GetUserAsync(context.Principal);

						if (user is null || user.LockoutEnd > DateTimeOffset.UtcNow) {
							context.Fail("This account is locked.");
						}
					},

					OnMessageReceived = context => {
						var accessToken = context.Request.Query["access_token"];

						var path = context.HttpContext.Request.Path;

						if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs")) {
							context.Token = accessToken;
						}

						return Task.CompletedTask;
					}
				};
			});

		return services;
	}
}
