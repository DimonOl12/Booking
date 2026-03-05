namespace EasyStay.Application.Interfaces;

public interface IScopeCoveredDbInicializer {
	Task InitializeAsync(CancellationToken cancellationToken = default);
}
