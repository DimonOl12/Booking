using Reservio.Application.Interfaces;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.Cities.Queries.GetAdvertisingPage;

public class GetCitiesAdvertisingPageQueryHandler(
	IPaginationService<CityAdvertisingVm, GetCitiesAdvertisingPageQuery> pagination
) : IRequestHandler<GetCitiesAdvertisingPageQuery, PageVm<CityAdvertisingVm>> {

	public Task<PageVm<CityAdvertisingVm>> Handle(GetCitiesAdvertisingPageQuery request, CancellationToken cancellationToken) =>
		 pagination.GetPageAsync(request, cancellationToken);
}

