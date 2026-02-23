using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.RoomAmenities.Queries.Shared;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.RoomAmenities.Queries.GetDetails;

public class GetRoomAmenityDetailsQueryHandler(
	IEasyStayDbContext context,
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
