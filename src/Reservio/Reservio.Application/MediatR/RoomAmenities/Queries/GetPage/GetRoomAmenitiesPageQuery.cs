using Reservio.Application.MediatR.RoomAmenities.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.RoomAmenities.Queries.GetPage;

public class GetRoomAmenitiesPageQuery : PaginationFilterDto, IRequest<PageVm<RoomAmenityVm>> {
	public string? Name { get; set; }
}

