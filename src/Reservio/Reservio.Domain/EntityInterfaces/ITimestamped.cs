namespace Reservio.Domain.EntityInterfaces;

public interface ITimestamped {
	public DateTime CreatedAtUtc { get; set; }

	public DateTime? UpdatedAtUtc { get; set; }
}

