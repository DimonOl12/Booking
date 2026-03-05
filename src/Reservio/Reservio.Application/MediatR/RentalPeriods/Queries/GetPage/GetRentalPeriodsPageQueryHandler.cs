using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.RentalPeriods.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.RentalPeriods.Queries.GetPage;

public class GetRentalPeriodsPageQueryHandler(
	IPaginationService<RentalPeriodVm, GetRentalPeriodsPageQuery> pagination
) : IRequestHandler<GetRentalPeriodsPageQuery, PageVm<RentalPeriodVm>> {

	public Task<PageVm<RentalPeriodVm>> Handle(GetRentalPeriodsPageQuery request, CancellationToken cancellationToken)
		=> pagination.GetPageAsync(request, cancellationToken);
}

