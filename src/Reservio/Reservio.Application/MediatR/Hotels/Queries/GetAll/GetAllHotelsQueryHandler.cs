using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Hotels.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.Hotels.Queries.GetAll;

public class GetAllHotelsQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllHotelsQuery, IEnumerable<HotelVm>> {

	public async Task<IEnumerable<HotelVm>> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken) {
		var items = await context.Hotels
			.AsNoTracking()
			.ProjectTo<HotelVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}

