using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Rooms.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Rooms.Queries.GetAll;

public class GetAllRoomsQueryHandler(
	IEasyStayDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllRoomsQuery, IEnumerable<RoomVm>> {

	public async Task<IEnumerable<RoomVm>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken) {
		var items = await context.Rooms
			.AsNoTracking()
			.ProjectTo<RoomVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}
