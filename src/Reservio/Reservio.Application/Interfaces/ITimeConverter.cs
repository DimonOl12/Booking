namespace Reservio.Application.Interfaces;

public interface ITimeConverter {
	DateTimeOffset ToDateTimeOffsetFromUtcTimeOnly(TimeOnly time);
}

