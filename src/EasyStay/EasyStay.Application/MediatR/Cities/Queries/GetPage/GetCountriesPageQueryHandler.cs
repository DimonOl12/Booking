using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Cities.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.Cities.Queries.GetPage;

public class GetCitiesPageQueryHandler(
	IPaginationService<CityVm, GetCitiesPageQuery> pagination
) : IRequestHandler<GetCitiesPageQuery, PageVm<CityVm>> {

	public Task<PageVm<CityVm>> Handle(GetCitiesPageQuery request, CancellationToken cancellationToken) =>
		 pagination.GetPageAsync(request, cancellationToken);
}
