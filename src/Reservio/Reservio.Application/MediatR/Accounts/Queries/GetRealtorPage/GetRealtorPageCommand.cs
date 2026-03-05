using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.Accounts.Queries.GetRealtorPage;

public class GetRealtorPageCommand : PaginationFilterDto, IRequest<PageVm<RealtorItemVm>> {
	public string? FirstName { get; set; }

	public string? LastName { get; set; }

	public bool? IsLocked { get; set; }
}

