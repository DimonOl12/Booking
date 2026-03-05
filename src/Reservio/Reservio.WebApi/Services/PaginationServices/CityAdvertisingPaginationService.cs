using AutoMapper;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Cities.Queries.GetAdvertisingPage;
using Reservio.Domain.Entities;

namespace Reservio.WebApi.Services.PaginationServices;

public class CityAdvertisingPaginationService(
	IReservioDbContext context,
	IMapper mapper
) : BasePaginationService<City, CityAdvertisingVm, GetCitiesAdvertisingPageQuery>(mapper) {

	protected override IQueryable<City> GetQuery() => context.Cities.OrderBy(c => c.Id);

	protected override IQueryable<CityAdvertisingVm> FilterQueryAfterProjectTo(IQueryable<CityAdvertisingVm> query, GetCitiesAdvertisingPageQuery filter) {
		if (filter.HasMinPrice == true)
			query = query.Where(c => c.MinPrice != null);

		return query;
	}
}

