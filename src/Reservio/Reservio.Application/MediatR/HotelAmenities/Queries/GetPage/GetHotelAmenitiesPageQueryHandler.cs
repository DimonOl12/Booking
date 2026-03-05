using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.HotelAmenities.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.HotelAmenities.Queries.GetPage;

public class GetHotelAmenitiesPageQueryHandler(
	IPaginationService<HotelAmenityVm, GetHotelAmenitiesPageQuery> pagination
) : IRequestHandler<GetHotelAmenitiesPageQuery, PageVm<HotelAmenityVm>> {

	public Task<PageVm<HotelAmenityVm>> Handle(GetHotelAmenitiesPageQuery request, CancellationToken cancellationToken)
		=> pagination.GetPageAsync(request, cancellationToken);
}

