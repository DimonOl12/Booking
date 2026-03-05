using EasyStay.Application.MediatR.Breakfasts.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.Breakfasts.Queries.GetPage;

public class GetBreakfastsPageQuery : PaginationFilterDto, IRequest<PageVm<BreakfastVm>> {
	public string? Name { get; set; }
}
