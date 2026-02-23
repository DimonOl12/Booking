using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Rooms.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.Rooms.Queries.GetPage;

public class GetRoomsPageQueryHandler(
	IPaginationService<RoomVm, GetRoomsPageQuery> pagination
) : IRequestHandler<GetRoomsPageQuery, PageVm<RoomVm>> {

	public Task<PageVm<RoomVm>> Handle(GetRoomsPageQuery request, CancellationToken cancellationToken) =>
		pagination.GetPageAsync(request, cancellationToken);
}
