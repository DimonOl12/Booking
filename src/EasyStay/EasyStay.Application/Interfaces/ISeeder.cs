namespace EasyStay.Application.Interfaces;

public interface ISeeder {
	Task SeedAsync(CancellationToken cancellationToken = default);
}
