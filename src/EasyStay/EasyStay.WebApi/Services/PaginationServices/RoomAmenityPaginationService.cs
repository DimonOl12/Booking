using AutoMapper;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.RoomAmenities.Queries.GetPage;
using EasyStay.Application.MediatR.RoomAmenities.Queries.Shared;
using EasyStay.Domain.Entities;

namespace EasyStay.WebApi.Services.PaginationServices;

public class RoomAmenityPaginationService(
	IEasyStayDbContext context,
	IMapper mapper
) : BasePaginationService<RoomAmenity, RoomAmenityVm, GetRoomAmenitiesPageQuery>(mapper) {

	protected override IQueryable<RoomAmenity> GetQuery() => context.RoomAmenities.OrderBy(ra => ra.Id);

	protected override IQueryable<RoomAmenity> FilterQueryBeforeProjectTo(IQueryable<RoomAmenity> query, GetRoomAmenitiesPageQuery filter) {
		if (filter.Name is not null)
			query = query.Where(ra => ra.Name.ToLower().Contains(filter.Name.ToLower()));

		return query;
	}
}
