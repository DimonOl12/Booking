using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.RoomAmenities.Queries.Shared;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.RoomAmenities.Queries.GetDetails;

public class GetRoomAmenityDetailsQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetRoomAmenityDetailsQuery, RoomAmenityVm> {

	public async Task<RoomAmenityVm> Handle(GetRoomAmenityDetailsQuery request, CancellationToken cancellationToken) {
		var vm = await context.RoomAmenities
			.AsNoTracking()
			.ProjectTo<RoomAmenityVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(ra => ra.Id == request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(RoomAmenity), request.Id);

		return vm;
	}
}

