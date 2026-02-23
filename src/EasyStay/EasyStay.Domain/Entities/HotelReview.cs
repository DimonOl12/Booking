using EasyStay.Domain.EntityInterfaces;

namespace EasyStay.Domain.Entities;

public class HotelReview : ITimestamped {
	public long Id { get; set; }

	public string Description { get; set; } = null!;

	public double? Score { get; set; }

	public DateTime CreatedAtUtc { get; set; }

	public DateTime? UpdatedAtUtc { get; set; }

	public long BookingId { get; set; }
	public Booking Booking { get; set; } = null!;
}
