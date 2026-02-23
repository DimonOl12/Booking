using EasyStay.Application.MediatR.RentalPeriods.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.RentalPeriods.Queries.GetPage;

public class GetRentalPeriodsPageQuery : PaginationFilterDto, IRequest<PageVm<RentalPeriodVm>> {
	public string? Name { get; set; }
}
