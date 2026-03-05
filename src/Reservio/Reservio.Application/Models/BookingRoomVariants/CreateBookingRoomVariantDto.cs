using Reservio.Application.Models.BookingBedSelections;

namespace Reservio.Application.Models.BookingRoomVariants;

public class CreateBookingRoomVariantDto {
	public int Quantity { get; set; }

	public long RoomVariantId { get; set; }

	public CreateBookingBedSelectionDto BookingBedSelection { get; set; } = null!;
}

