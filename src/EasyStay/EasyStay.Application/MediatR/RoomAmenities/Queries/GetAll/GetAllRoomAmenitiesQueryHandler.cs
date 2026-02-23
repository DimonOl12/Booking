using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.RoomAmenities.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.RoomAmenities.Queries.GetAll;

public class GetAllRoomAmenitiesQueryHandler(
	IEasyStayDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllRoomAmenitiesQuery, IEnumerable<RoomAmenityVm>> {

	public async Task<IEnumerable<RoomAmenityVm>> Handle(GetAllRoomAmenitiesQuery request, CancellationToken cancellationToken) {
		var items = await context.RoomAmenities
			.AsNoTracking()
			.ProjectTo<RoomAmenityVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}
