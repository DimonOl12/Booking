using Reservio.Application.Interfaces;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.Accounts.Queries.GetRealtorPage;

public class GetRealtorPageCommandHandler(
	IPaginationService<RealtorItemVm, GetRealtorPageCommand> paginationService
) : IRequestHandler<GetRealtorPageCommand, PageVm<RealtorItemVm>> {

	public Task<PageVm<RealtorItemVm>> Handle(GetRealtorPageCommand request, CancellationToken cancellationToken) =>
		paginationService.GetPageAsync(request, cancellationToken);
}

