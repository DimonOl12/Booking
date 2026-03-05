using EasyStay.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyStay.Infrastructure.Services;

public class AggregateSeeder(
	IServiceScopeFactory serviceScopeFactory,
	IConfiguration configuration
) : IAggregateSeeder {

	public async Task SeedAsync(CancellationToken cancellationToken = default) {
		await using var scope = serviceScopeFactory.CreateAsyncScope();
		var serviceProvider = scope.ServiceProvider;

		if (configuration.GetValue<bool>("SeedCleanData"))
			await serviceProvider.GetRequiredService<ICleanDataSeeder>().SeedAsync(cancellationToken);

		if (configuration.GetValue<bool>("SeedGeneratedData"))
			await serviceProvider.GetRequiredService<IGeneratedDataSeeder>().SeedAsync(cancellationToken);

		await Console.Out.WriteLineAsync("Seedind completed");
	}
}
