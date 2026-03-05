using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.RentalPeriods.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.RentalPeriods.Queries.GetPage;

public class GetRentalPeriodsPageQueryHandler(
	IPaginationService<RentalPeriodVm, GetRentalPeriodsPageQuery> pagination
) : IRequestHandler<GetRentalPeriodsPageQuery, PageVm<RentalPeriodVm>> {

	public Task<PageVm<RentalPeriodVm>> Handle(GetRentalPeriodsPageQuery request, CancellationToken cancellationToken)
		=> pagination.GetPageAsync(request, cancellationToken);
}
