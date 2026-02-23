using EasyStay.Application.MediatR.RoomTypes.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.RoomTypes.Queries.GetPage;

public class GetRoomTypesPageQuery : PaginationFilterDto, IRequest<PageVm<RoomTypeVm>> {
	public string? Name { get; set; }
}
