using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Countries.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.Countries.Queries.GetPage;

public class GetCountriesPageQueryHandler(
	IPaginationService<CountryVm, GetCountriesPageQuery> pagination
) : IRequestHandler<GetCountriesPageQuery, PageVm<CountryVm>> {

	public Task<PageVm<CountryVm>> Handle(GetCountriesPageQuery request, CancellationToken cancellationToken)
		=> pagination.GetPageAsync(request, cancellationToken);
}

