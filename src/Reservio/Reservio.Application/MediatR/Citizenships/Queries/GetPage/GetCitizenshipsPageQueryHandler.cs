using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Citizenships.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.Citizenships.Queries.GetPage;

public class GetCitizenshipsPageQueryHandler(
	IPaginationService<CitizenshipVm, GetCitizenshipsPageQuery> pagination
) : IRequestHandler<GetCitizenshipsPageQuery, PageVm<CitizenshipVm>> {

	public Task<PageVm<CitizenshipVm>> Handle(GetCitizenshipsPageQuery request, CancellationToken cancellationToken)
		=> pagination.GetPageAsync(request, cancellationToken);
}

