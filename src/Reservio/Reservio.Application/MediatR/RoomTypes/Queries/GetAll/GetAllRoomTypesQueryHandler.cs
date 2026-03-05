using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.RoomTypes.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.RoomTypes.Queries.GetAll;

public class GetAllRoomTypesQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllRoomTypesQuery, IEnumerable<RoomTypeVm>> {

	public async Task<IEnumerable<RoomTypeVm>> Handle(GetAllRoomTypesQuery request, CancellationToken cancellationToken) {
		var items = await context.RoomTypes
			.AsNoTracking()
			.ProjectTo<RoomTypeVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}

