using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Cities.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.Cities.Queries.GetAll;

public class GetAllCitiesQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllCitiesQuery, IEnumerable<CityVm>> {

	public async Task<IEnumerable<CityVm>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken) {
		var items = await context.Cities
			.ProjectTo<CityVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}

