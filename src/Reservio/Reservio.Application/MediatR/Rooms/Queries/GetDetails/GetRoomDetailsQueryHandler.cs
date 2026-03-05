using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Rooms.Queries.Shared;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.Rooms.Queries.GetDetails;

public class GetRoomDetailsQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetRoomDetailsQuery, RoomVm> {

	public async Task<RoomVm> Handle(GetRoomDetailsQuery request, CancellationToken cancellationToken) {
		var vm = await context.Rooms
			.AsNoTracking()
			.ProjectTo<RoomVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(Room), request.Id);

		return vm;
	}
}

