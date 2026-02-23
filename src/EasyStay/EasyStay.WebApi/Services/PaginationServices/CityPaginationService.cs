using AutoMapper;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Cities.Queries.GetPage;
using EasyStay.Application.MediatR.Cities.Queries.Shared;
using EasyStay.Domain.Entities;

namespace EasyStay.WebApi.Services.PaginationServices;

public class CityPaginationService(
	IEasyStayDbContext context,
	IMapper mapper
) : BasePaginationService<City, CityVm, GetCitiesPageQuery>(mapper) {

	protected override IQueryable<City> GetQuery() => context.Cities;

	protected override IQueryable<City> FilterQueryBeforeProjectTo(IQueryable<City> query, GetCitiesPageQuery filter) {
		if (filter.IsRandomItems == true) {
			query = query.OrderBy(c => Guid.NewGuid());
		}
		else {
			query = query.OrderBy(c => c.Id);
		}

		if (filter.Name is not null)
			query = query.Where(c => c.Name.ToLower().Contains(filter.Name.ToLower()));

		if (filter.Longitude is not null)
			query = query.Where(c => c.Longitude == filter.Longitude);

		if (filter.Latitude is not null)
			query = query.Where(c => c.Latitude == filter.Latitude);

		if (filter.MinLongitude is not null)
			query = query.Where(c => c.Longitude >= filter.MinLongitude);
		if (filter.MaxLongitude is not null)
			query = query.Where(c => c.Longitude <= filter.MaxLongitude);

		if (filter.MinLatitude is not null)
			query = query.Where(c => c.Latitude >= filter.MinLatitude);
		if (filter.MaxLatitude is not null)
			query = query.Where(c => c.Latitude <= filter.MaxLatitude);

		if (filter.CountryId is not null)
			query = query.Where(c => c.CountryId == filter.CountryId);

		return query;
	}
}
