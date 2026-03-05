using EasyStay.Application.MediatR.RoomAmenities.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.RoomAmenities.Queries.GetPage;

public class GetRoomAmenitiesPageQuery : PaginationFilterDto, IRequest<PageVm<RoomAmenityVm>> {
	public string? Name { get; set; }
}
