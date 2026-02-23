using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Countries.Queries.Shared;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Countries.Queries.GetDetails;

public class GetCountryDetailsQueryHandler(
	IEasyStayDbContext context,
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
