using EasyStay.Application;
using EasyStay.Application.Common.Mappings;
using EasyStay.Application.Interfaces;
using EasyStay.Infrastructure;
using EasyStay.Infrastructure.Services;
using EasyStay.WebApi.Extensions;
using EasyStay.WebApi.Hubs;
using EasyStay.WebApi.Middleware;
using EasyStay.WebApi.Services;
using EasyStay.WebApi.Services.PaginationServices.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(
	configAction => {
		configAction.AddProfile(new AssemblyMappingProfile(typeof(IEasyStayDbContext).Assembly));
	}
);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
	options.AddSecurityDefinition(
		"Bearer",
		new OpenApiSecurityScheme {
			Description = "Jwt Auth header using the Bearer scheme",
			Type = SecuritySchemeType.Http,
			Scheme = "bearer"
		}
	);
	options.AddSecurityRequirement(new OpenApiSecurityRequirement {
		{
			new OpenApiSecurityScheme {
				Reference = new OpenApiReference {
					Id = "Bearer",
					Type = ReferenceType.SecurityScheme
				}
			},
			new List<string>()
		}
	});
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IIdentityValidator, IdentityValidator>();
builder.Services.AddScoped<ICollectionValidator, CollectionValidator>();
builder.Services.AddSingleton<IImageValidator, ImageValidator>();
builder.Services.AddScoped<IExistingEntityCheckerService, ExistingEntityCheckerService>();
builder.Services.AddSingleton<ITimeConverter, TimeConverter>();
builder.Services.AddSingleton<IDateConverter, DateConverter>();
builder.Services.AddSingleton<IImageSeeder, LoremPicsumImageSeeder>();

builder.Services.AddPaginationServices();

builder.Services.AddCors(options => options.AddPolicy("SignalRCors", configuration => {
	configuration
		.WithOrigins(builder.Configuration["FrontEndDomain"] ?? throw new NullReferenceException("FrontEndDomain"))
		.AllowAnyHeader()
		.AllowAnyMethod()
		.AllowCredentials();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsDocker()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();

app.UseCors(
	configuration => configuration
		.AllowAnyOrigin()
		.AllowAnyHeader()
		.AllowAnyMethod()
);

var imageService = app.Services.GetRequiredService<IImageService>();
imageService.CreateImagesDirIfNotExists();

app.UseStaticFiles(new StaticFileOptions {
	FileProvider = new PhysicalFileProvider(imageService.ImagesDir),
	RequestPath = "/images"
});

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/hubs/chat")
	.RequireCors("SignalRCors");

app.MapControllers();

await app.Services.GetRequiredService<IScopeCoveredDbInicializer>().InitializeAsync();

var seedTask = app.Services.GetRequiredService<IAggregateSeeder>().SeedAsync();
var appTask = app.RunAsync();

await seedTask;
await appTask;
