using EasyStay.Application.Interfaces;

namespace EasyStay.WebApi.Services;

public class DateConverter : IDateConverter {
	public DateOnly ToFirstDayOfMonth(DateOnly date) {
		return new DateOnly(date.Year, date.Month, 1);
	}
}
