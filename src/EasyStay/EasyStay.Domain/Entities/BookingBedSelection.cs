namespace EasyStay.Domain.Entities;

public class BookingBedSelection {
	public long BookingRoomVariantId { get; set; }
	public BookingRoomVariant BookingRoomVariant { get; set; } = null!;

	public bool IsSingleBed { get; set; }

	public bool IsDoubleBed { get; set; }

	public bool IsExtraBed { get; set; }

	public bool IsSofa { get; set; }

	public bool IsKingsizeBed { get; set; }
}
