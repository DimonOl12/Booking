using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Cities.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Cities.Queries.GetAll;

public class GetAllCitiesQueryHandler(
	IEasyStayDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllCitiesQuery, IEnumerable<CityVm>> {

	public async Task<IEnumerable<CityVm>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken) {
		var items = await context.Cities
			.ProjectTo<CityVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}
