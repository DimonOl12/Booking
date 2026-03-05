using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Countries.Queries.Shared;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.Countries.Queries.GetDetails;

public class GetCountryDetailsQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetCountryDetailsQuery, CountryVm> {

	public async Task<CountryVm> Handle(GetCountryDetailsQuery request, CancellationToken cancellationToken) {
		var vm = await context.Countries
			.ProjectTo<CountryVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(Country), request.Id);

		return vm;
	}
}

