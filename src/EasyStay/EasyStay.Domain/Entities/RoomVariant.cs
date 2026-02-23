namespace EasyStay.Domain.Entities;

public class RoomVariant {
	public long Id { get; set; }

	public decimal Price { get; set; }

	public decimal? DiscountPrice { get; set; }

	public long RoomId { get; set; }
	public Room Room { get; set; } = null!;

	public GuestInfo GuestInfo { get; set; } = null!;

	public BedInfo BedInfo { get; set; } = null!;

	public ICollection<BookingRoomVariant> BookingRoomVariants { get; set; } = null!;
}
