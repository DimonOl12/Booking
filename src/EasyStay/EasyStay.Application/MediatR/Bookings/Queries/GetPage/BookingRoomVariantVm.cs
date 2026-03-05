using EasyStay.Application.Common.Mappings;
using EasyStay.Application.MediatR.RoomVariants.Queries.Shared;
using EasyStay.Domain.Entities;

namespace EasyStay.Application.MediatR.Bookings.Queries.GetPage;

public class BookingRoomVariantVm : IMapWith<BookingRoomVariant> {
	public long Id { get; set; }

	public int Quantity { get; set; }

	public long RoomVariantId { get; set; }
	public RoomVariantVm RoomVariant { get; set; } = null!;

	public BookingBedSelectionVm BookingBedSelection { get; set; } = null!;
}
