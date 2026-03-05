using AutoMapper;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.RoomTypes.Queries.GetPage;
using Reservio.Application.MediatR.RoomTypes.Queries.Shared;
using Reservio.Domain.Entities;

namespace Reservio.WebApi.Services.PaginationServices;

public class RoomTypePaginationService(
	IReservioDbContext context,
	IMapper mapper
) : BasePaginationService<RoomType, RoomTypeVm, GetRoomTypesPageQuery>(mapper) {

	protected override IQueryable<RoomType> GetQuery() => context.RoomTypes.OrderBy(rt => rt.Id);

	protected override IQueryable<RoomType> FilterQueryBeforeProjectTo(IQueryable<RoomType> query, GetRoomTypesPageQuery filter) {
		if (filter.Name is not null)
			query = query.Where(rt => rt.Name.ToLower().Contains(filter.Name.ToLower()));

		return query;
	}
}

