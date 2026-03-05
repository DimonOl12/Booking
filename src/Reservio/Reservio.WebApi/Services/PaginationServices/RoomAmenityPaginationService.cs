using AutoMapper;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.RoomAmenities.Queries.GetPage;
using Reservio.Application.MediatR.RoomAmenities.Queries.Shared;
using Reservio.Domain.Entities;

namespace Reservio.WebApi.Services.PaginationServices;

public class RoomAmenityPaginationService(
	IReservioDbContext context,
	IMapper mapper
) : BasePaginationService<RoomAmenity, RoomAmenityVm, GetRoomAmenitiesPageQuery>(mapper) {

	protected override IQueryable<RoomAmenity> GetQuery() => context.RoomAmenities.OrderBy(ra => ra.Id);

	protected override IQueryable<RoomAmenity> FilterQueryBeforeProjectTo(IQueryable<RoomAmenity> query, GetRoomAmenitiesPageQuery filter) {
		if (filter.Name is not null)
			query = query.Where(ra => ra.Name.ToLower().Contains(filter.Name.ToLower()));

		return query;
	}
}

