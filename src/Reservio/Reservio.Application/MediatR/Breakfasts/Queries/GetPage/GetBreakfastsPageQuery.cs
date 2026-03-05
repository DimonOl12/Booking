using Reservio.Application.MediatR.Breakfasts.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.Breakfasts.Queries.GetPage;

public class GetBreakfastsPageQuery : PaginationFilterDto, IRequest<PageVm<BreakfastVm>> {
	public string? Name { get; set; }
}

