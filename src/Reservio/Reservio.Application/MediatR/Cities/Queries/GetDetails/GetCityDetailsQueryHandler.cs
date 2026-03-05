using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Cities.Queries.Shared;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.Cities.Queries.GetDetails;

public class GetCityDetailsQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetCityDetailsQuery, CityVm> {

	public async Task<CityVm> Handle(GetCityDetailsQuery request, CancellationToken cancellationToken) {
		var vm = await context.Cities
			.ProjectTo<CityVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(City), request.Id);

		return vm;
	}
}

