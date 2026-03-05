using AutoMapper;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Countries.Queries.GetPage;
using EasyStay.Application.MediatR.Countries.Queries.Shared;
using EasyStay.Domain.Entities;

namespace EasyStay.WebApi.Services.PaginationServices;

public class CountryPaginationService(
	IEasyStayDbContext context,
	IMapper mapper
) : BasePaginationService<Country, CountryVm, GetCountriesPageQuery>(mapper) {

	protected override IQueryable<Country> GetQuery() => context.Countries.OrderBy(c => c.Id);

	protected override IQueryable<Country> FilterQueryBeforeProjectTo(IQueryable<Country> query, GetCountriesPageQuery filter) {
		if (filter.Name is not null)
			query = query.Where(c => c.Name.ToLower().Contains(filter.Name.ToLower()));

		return query;
	}
}
