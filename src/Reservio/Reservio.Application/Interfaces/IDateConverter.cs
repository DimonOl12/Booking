namespace Reservio.Application.Interfaces;

public interface IDateConverter {
	DateOnly ToFirstDayOfMonth(DateOnly date);
}

