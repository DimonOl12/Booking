using AutoMapper;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.HotelAmenities.Queries.GetPage;
using Reservio.Application.MediatR.HotelAmenities.Queries.Shared;
using Reservio.Domain.Entities;

namespace Reservio.WebApi.Services.PaginationServices;

public class HotelAmenityPaginationService(
	IReservioDbContext context,
	IMapper mapper
) : BasePaginationService<HotelAmenity, HotelAmenityVm, GetHotelAmenitiesPageQuery>(mapper) {

	protected override IQueryable<HotelAmenity> GetQuery() => context.HotelAmenities.OrderBy(ha => ha.Id);

	protected override IQueryable<HotelAmenity> FilterQueryBeforeProjectTo(IQueryable<HotelAmenity> query, GetHotelAmenitiesPageQuery filter) {
		if (filter.Name is not null)
			query = query.Where(ha => ha.Name.ToLower().Contains(filter.Name.ToLower()));

		return query;
	}
}

