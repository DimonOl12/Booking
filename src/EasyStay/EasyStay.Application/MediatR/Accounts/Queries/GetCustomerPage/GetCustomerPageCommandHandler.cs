using EasyStay.Application.Interfaces;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.Accounts.Queries.GetCustomerPage;

public class GetCustomerPageCommandHandler(
	IPaginationService<CustomerItemVm, GetCustomerPageCommand> paginationService
) : IRequestHandler<GetCustomerPageCommand, PageVm<CustomerItemVm>> {

	public Task<PageVm<CustomerItemVm>> Handle(GetCustomerPageCommand request, CancellationToken cancellationToken) =>
		paginationService.GetPageAsync(request, cancellationToken);
}
