using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.RoomAmenities.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.RoomAmenities.Queries.GetPage;

public class GetRoomAmenitiesPageQueryHandler(
	IPaginationService<RoomAmenityVm, GetRoomAmenitiesPageQuery> pagination
) : IRequestHandler<GetRoomAmenitiesPageQuery, PageVm<RoomAmenityVm>> {

	public Task<PageVm<RoomAmenityVm>> Handle(GetRoomAmenitiesPageQuery request, CancellationToken cancellationToken)
		=> pagination.GetPageAsync(request, cancellationToken);
}

