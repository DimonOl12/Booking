using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.RoomAmenities.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.RoomAmenities.Queries.GetAll;

public class GetAllRoomAmenitiesQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllRoomAmenitiesQuery, IEnumerable<RoomAmenityVm>> {

	public async Task<IEnumerable<RoomAmenityVm>> Handle(GetAllRoomAmenitiesQuery request, CancellationToken cancellationToken) {
		var items = await context.RoomAmenities
			.AsNoTracking()
			.ProjectTo<RoomAmenityVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}

