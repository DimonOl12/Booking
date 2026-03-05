using AutoMapper;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Cities.Queries.GetAdvertisingPage;
using EasyStay.Domain.Entities;

namespace EasyStay.WebApi.Services.PaginationServices;

public class CityAdvertisingPaginationService(
	IEasyStayDbContext context,
	IMapper mapper
) : BasePaginationService<City, CityAdvertisingVm, GetCitiesAdvertisingPageQuery>(mapper) {

	protected override IQueryable<City> GetQuery() => context.Cities.OrderBy(c => c.Id);

	protected override IQueryable<CityAdvertisingVm> FilterQueryAfterProjectTo(IQueryable<CityAdvertisingVm> query, GetCitiesAdvertisingPageQuery filter) {
		if (filter.HasMinPrice == true)
			query = query.Where(c => c.MinPrice != null);

		return query;
	}
}
