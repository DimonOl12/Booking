using Reservio.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Reservio.Infrastructure.Services;

public class ScopeCoveredDbInicializer(
	IServiceScopeFactory serviceScopeFactory
) : IScopeCoveredDbInicializer {

	public async Task InitializeAsync(CancellationToken cancellationToken = default) {
		await using var scope = serviceScopeFactory.CreateAsyncScope();
		var serviceProvider = scope.ServiceProvider;

		await serviceProvider.GetRequiredService<IDbInicializer>().InitializeAsync(cancellationToken);
	}
}

