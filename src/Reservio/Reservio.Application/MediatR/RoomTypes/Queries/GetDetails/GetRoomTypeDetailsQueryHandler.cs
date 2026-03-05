using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.RoomTypes.Queries.Shared;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.RoomTypes.Queries.GetDetails;

public class GetRoomTypeDetailsQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetRoomTypeDetailsQuery, RoomTypeVm> {

	public async Task<RoomTypeVm> Handle(GetRoomTypeDetailsQuery request, CancellationToken cancellationToken) {
		var vm = await context.RoomTypes
			.AsNoTracking()
			.ProjectTo<RoomTypeVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(rt => rt.Id == request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(RoomType), request.Id);

		return vm;
	}
}

