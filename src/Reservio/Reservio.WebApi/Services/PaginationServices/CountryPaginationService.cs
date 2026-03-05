using AutoMapper;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Countries.Queries.GetPage;
using Reservio.Application.MediatR.Countries.Queries.Shared;
using Reservio.Domain.Entities;

namespace Reservio.WebApi.Services.PaginationServices;

public class CountryPaginationService(
	IReservioDbContext context,
	IMapper mapper
) : BasePaginationService<Country, CountryVm, GetCountriesPageQuery>(mapper) {

	protected override IQueryable<Country> GetQuery() => context.Countries.OrderBy(c => c.Id);

	protected override IQueryable<Country> FilterQueryBeforeProjectTo(IQueryable<Country> query, GetCountriesPageQuery filter) {
		if (filter.Name is not null)
			query = query.Where(c => c.Name.ToLower().Contains(filter.Name.ToLower()));

		return query;
	}
}

