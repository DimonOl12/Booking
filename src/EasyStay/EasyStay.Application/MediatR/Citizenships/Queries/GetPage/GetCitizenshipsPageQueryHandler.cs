using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Citizenships.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.Citizenships.Queries.GetPage;

public class GetCitizenshipsPageQueryHandler(
	IPaginationService<CitizenshipVm, GetCitizenshipsPageQuery> pagination
) : IRequestHandler<GetCitizenshipsPageQuery, PageVm<CitizenshipVm>> {

	public Task<PageVm<CitizenshipVm>> Handle(GetCitizenshipsPageQuery request, CancellationToken cancellationToken)
		=> pagination.GetPageAsync(request, cancellationToken);
}
