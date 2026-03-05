using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.RoomTypes.Queries.Shared;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.RoomTypes.Queries.GetDetails;

public class GetRoomTypeDetailsQueryHandler(
	IEasyStayDbContext context,
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
