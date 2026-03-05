using Reservio.Application.MediatR.RentalPeriods.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.RentalPeriods.Queries.GetPage;

public class GetRentalPeriodsPageQuery : PaginationFilterDto, IRequest<PageVm<RentalPeriodVm>> {
	public string? Name { get; set; }
}

