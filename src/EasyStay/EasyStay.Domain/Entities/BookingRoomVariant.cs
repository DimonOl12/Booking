namespace EasyStay.Domain.Entities;

public class BookingRoomVariant {
	public long Id { get; set; }

	public int Quantity { get; set; }

	public long RoomVariantId { get; set; }
	public RoomVariant RoomVariant { get; set; } = null!;

	public long BookingId { get; set; }
	public Booking Booking { get; set; } = null!;

	public BookingBedSelection BookingBedSelection { get; set; } = null!;
}
