using Reservio.Application.Interfaces;

namespace Reservio.WebApi.Services.PaginationServices.DependencyInjection;

public static class PaginationServicesDependencyInjection {
	public static IServiceCollection AddPaginationServices(this IServiceCollection services) {
		var assembly = typeof(PaginationServicesDependencyInjection).Assembly;

		var paginationServiceTypes = assembly.GetExportedTypes()
			.Where(type => !type.IsAbstract && !type.IsInterface)
			.SelectMany(type => type.GetInterfaces(), (type, @interface) => new { type, @interface })
			.Where(pair =>
				pair.@interface.IsGenericType &&
				pair.@interface.GetGenericTypeDefinition() == typeof(IPaginationService<,>))
			.ToArray();

		foreach (var pair in paginationServiceTypes) {
			services.AddScoped(pair.@interface, pair.type);
		}

		return services;
	}
}

