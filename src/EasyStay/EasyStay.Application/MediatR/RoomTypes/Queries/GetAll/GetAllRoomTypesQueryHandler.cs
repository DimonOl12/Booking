using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.RoomTypes.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.RoomTypes.Queries.GetAll;

public class GetAllRoomTypesQueryHandler(
	IEasyStayDbContext context,
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
