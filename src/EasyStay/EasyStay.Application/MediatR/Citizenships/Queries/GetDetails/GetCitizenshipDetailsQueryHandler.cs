using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Citizenships.Queries.Shared;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Citizenships.Queries.GetDetails;

public class GetCitizenshipDetailsQueryHandler(
	IEasyStayDbContext context,
	IMapper mapper
) : IRequestHandler<GetCitizenshipDetailsQuery, CitizenshipVm> {

	public async Task<CitizenshipVm> Handle(GetCitizenshipDetailsQuery request, CancellationToken cancellationToken) {
		var vm = await context.Citizenships
			.AsNoTracking()
			.ProjectTo<CitizenshipVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(Citizenship), request.Id);

		return vm;
	}
}
