using Reservio.Application.MediatR.RoomTypes.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.RoomTypes.Queries.GetPage;

public class GetRoomTypesPageQuery : PaginationFilterDto, IRequest<PageVm<RoomTypeVm>> {
	public string? Name { get; set; }
}

