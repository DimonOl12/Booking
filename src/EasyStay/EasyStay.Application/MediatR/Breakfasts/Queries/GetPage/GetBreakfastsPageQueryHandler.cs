using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Breakfasts.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.Breakfasts.Queries.GetPage;

public class GetBreakfastsPageQueryHandler(
	IPaginationService<BreakfastVm, GetBreakfastsPageQuery> pagination
) : IRequestHandler<GetBreakfastsPageQuery, PageVm<BreakfastVm>> {

	public Task<PageVm<BreakfastVm>> Handle(GetBreakfastsPageQuery request, CancellationToken cancellationToken)
		=> pagination.GetPageAsync(request, cancellationToken);
}
