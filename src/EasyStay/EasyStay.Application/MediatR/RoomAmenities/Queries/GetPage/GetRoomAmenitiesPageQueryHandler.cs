using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.RoomAmenities.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.RoomAmenities.Queries.GetPage;

public class GetRoomAmenitiesPageQueryHandler(
	IPaginationService<RoomAmenityVm, GetRoomAmenitiesPageQuery> pagination
) : IRequestHandler<GetRoomAmenitiesPageQuery, PageVm<RoomAmenityVm>> {

	public Task<PageVm<RoomAmenityVm>> Handle(GetRoomAmenitiesPageQuery request, CancellationToken cancellationToken)
		=> pagination.GetPageAsync(request, cancellationToken);
}
