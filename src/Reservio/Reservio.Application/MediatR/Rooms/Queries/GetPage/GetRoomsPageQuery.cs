using Reservio.Application.MediatR.Rooms.Queries.Shared;
using Reservio.Application.Models.Date;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.Rooms.Queries.GetPage;

public class GetRoomsPageQuery : PaginationFilterDto, IRequest<PageVm<RoomVm>> {
	public string? Name { get; set; }

	public double? Area { get; set; }
	public double? MinArea { get; set; }
	public double? MaxArea { get; set; }

	public int? NumberOfRooms { get; set; }
	public int? MinNumberOfRooms { get; set; }
	public int? MaxNumberOfRooms { get; set; }

	public int? Quantity { get; set; }
	public int? MinQuantity { get; set; }
	public int? MaxQuantity { get; set; }

	public FreeDatePeriod? FreePeriod { get; set; }

	public long? HotelId { get; set; }

	public long? RoomTypeId { get; set; }

	public IEnumerable<long>? AllRentalPeriodIds { get; set; } = null!;
	public IEnumerable<long>? AnyRentalPeriodIds { get; set; } = null!;

	public IEnumerable<long>? AllAmenityIds { get; set; } = null!;
	public IEnumerable<long>? AnyAmenityIds { get; set; } = null!;
}

