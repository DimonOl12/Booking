namespace EasyStay.Domain.Entities;

public class GuestInfo {
	public long RoomVariantId { get; set; }
	public RoomVariant RoomVariant { get; set; } = null!;

	public int AdultCount { get; set; }

	public int ChildCount { get; set; }
}
