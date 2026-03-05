using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Citizenships.Queries.Shared;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.Citizenships.Queries.GetDetails;

public class GetCitizenshipDetailsQueryHandler(
	IReservioDbContext context,
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

