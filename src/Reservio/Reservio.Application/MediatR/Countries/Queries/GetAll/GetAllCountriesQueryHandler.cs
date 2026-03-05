using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Countries.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.Countries.Queries.GetAll;

public class GetAllCountriesQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllCountriesQuery, IEnumerable<CountryVm>> {

	public async Task<IEnumerable<CountryVm>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken) {
		var items = await context.Countries
			.ProjectTo<CountryVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}

