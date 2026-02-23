using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Cities.Queries.Shared;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Cities.Queries.GetDetails;

public class GetCityDetailsQueryHandler(
	IEasyStayDbContext context,
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
