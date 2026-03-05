using EasyStay.Application.Models.BookingBedSelections;

namespace EasyStay.Application.Models.BookingRoomVariants;

public class CreateBookingRoomVariantDto {
	public int Quantity { get; set; }

	public long RoomVariantId { get; set; }

	public CreateBookingBedSelectionDto BookingBedSelection { get; set; } = null!;
}
