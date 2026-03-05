using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Breakfasts.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.Breakfasts.Queries.GetPage;

public class GetBreakfastsPageQueryHandler(
	IPaginationService<BreakfastVm, GetBreakfastsPageQuery> pagination
) : IRequestHandler<GetBreakfastsPageQuery, PageVm<BreakfastVm>> {

	public Task<PageVm<BreakfastVm>> Handle(GetBreakfastsPageQuery request, CancellationToken cancellationToken)
		=> pagination.GetPageAsync(request, cancellationToken);
}

