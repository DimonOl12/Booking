namespace EasyStay.Application.Interfaces;

public interface IDateConverter {
	DateOnly ToFirstDayOfMonth(DateOnly date);
}
