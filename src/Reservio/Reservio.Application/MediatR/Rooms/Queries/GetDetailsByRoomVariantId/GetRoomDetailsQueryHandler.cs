using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Rooms.Queries.Shared;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.Rooms.Queries.GetDetailsByRoomVariantId;

public class GetDetailsByRoomVariantIdQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetDetailsByRoomVariantIdQuery, RoomVm> {

	public async Task<RoomVm> Handle(GetDetailsByRoomVariantIdQuery request, CancellationToken cancellationToken) {
		var vm = await context.Rooms
			.AsNoTracking()
			.ProjectTo<RoomVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(r => r.Variants.Any(v => v.Id == request.RoomVariantId), cancellationToken)
			?? throw new NotFoundException(nameof(RoomVariant), request.RoomVariantId);

		return vm;
	}
}

