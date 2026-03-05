using EasyStay.Application.Interfaces;

namespace EasyStay.WebApi.Services;

public class TimeConverter : ITimeConverter {
	public DateTimeOffset ToDateTimeOffsetFromUtcTimeOnly(TimeOnly time) {
		return new DateTimeOffset(time.Ticks, TimeSpan.Zero);
	}
}
