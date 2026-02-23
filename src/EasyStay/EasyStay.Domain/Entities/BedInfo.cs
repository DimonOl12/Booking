namespace EasyStay.Domain.Entities;

public class BedInfo {
	public long RoomVariantId { get; set; }
	public RoomVariant RoomVariant { get; set; } = null!;

	public int SingleBedCount { get; set; }

	public int DoubleBedCount { get; set; }

	public int ExtraBedCount { get; set; }

	public int SofaCount { get; set; }

	public int KingsizeBedCount { get; set; }
}
