using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Rooms.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.Rooms.Queries.GetPage;

public class GetRoomsPageQueryHandler(
	IPaginationService<RoomVm, GetRoomsPageQuery> pagination
) : IRequestHandler<GetRoomsPageQuery, PageVm<RoomVm>> {

	public Task<PageVm<RoomVm>> Handle(GetRoomsPageQuery request, CancellationToken cancellationToken) =>
		pagination.GetPageAsync(request, cancellationToken);
}

