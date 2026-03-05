using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.RoomTypes.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.RoomTypes.Queries.GetPage;

public class GetRoomTypesPageQueryHandler(
	IPaginationService<RoomTypeVm, GetRoomTypesPageQuery> pagination
) : IRequestHandler<GetRoomTypesPageQuery, PageVm<RoomTypeVm>> {

	public Task<PageVm<RoomTypeVm>> Handle(GetRoomTypesPageQuery request, CancellationToken cancellationToken)
		=> pagination.GetPageAsync(request, cancellationToken);
}

