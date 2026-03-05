using AutoMapper;
using Reservio.Application.MediatR.Accounts.Queries.GetCustomerPage;
using Reservio.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Reservio.WebApi.Services.PaginationServices;

public class CustomerPaginationService(
	UserManager<User> customerManager,
	IMapper mapper
) : BasePaginationService<Customer, CustomerItemVm, GetCustomerPageCommand>(mapper) {

	protected override IQueryable<Customer> GetQuery() =>
		customerManager.Users
			.OfType<Customer>()
			.OrderBy(c => c.Id);

	protected override IQueryable<Customer> FilterQueryBeforeProjectTo(IQueryable<Customer> query, GetCustomerPageCommand filter) {
		if (filter.FirstName is not null)
			query = query.Where(c => c.FirstName.ToLower().Contains(filter.FirstName.ToLower()));

		if (filter.LastName is not null)
			query = query.Where(c => c.LastName.ToLower().Contains(filter.LastName.ToLower()));

		if (filter.IsLocked is not null)
			query = query.Where(c => (c.LockoutEnd != null && c.LockoutEnd >= DateTimeOffset.UtcNow) == filter.IsLocked);

		return query;
	}
}

