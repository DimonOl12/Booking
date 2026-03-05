using Reservio.Application.Common.Mappings;
using Reservio.Application.MediatR.RoomVariants.Queries.Shared;
using Reservio.Domain.Entities;

namespace Reservio.Application.MediatR.Bookings.Queries.GetPage;

public class BookingRoomVariantVm : IMapWith<BookingRoomVariant> {
	public long Id { get; set; }

	public int Quantity { get; set; }

	public long RoomVariantId { get; set; }
	public RoomVariantVm RoomVariant { get; set; } = null!;

	public BookingBedSelectionVm BookingBedSelection { get; set; } = null!;
}

