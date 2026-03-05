namespace Reservio.Application.Interfaces;

public interface IDbInicializer {
	Task InitializeAsync(CancellationToken cancellationToken = default);
}

