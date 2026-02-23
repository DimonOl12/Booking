using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Rooms.Queries.Shared;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Rooms.Queries.GetDetailsByRoomVariantId;

public class GetDetailsByRoomVariantIdQueryHandler(
	IEasyStayDbContext context,
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
