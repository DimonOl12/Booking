using EasyStay.Application.Interfaces;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.Cities.Queries.GetAdvertisingPage;

public class GetCitiesAdvertisingPageQueryHandler(
	IPaginationService<CityAdvertisingVm, GetCitiesAdvertisingPageQuery> pagination
) : IRequestHandler<GetCitiesAdvertisingPageQuery, PageVm<CityAdvertisingVm>> {

	public Task<PageVm<CityAdvertisingVm>> Handle(GetCitiesAdvertisingPageQuery request, CancellationToken cancellationToken) =>
		 pagination.GetPageAsync(request, cancellationToken);
}
