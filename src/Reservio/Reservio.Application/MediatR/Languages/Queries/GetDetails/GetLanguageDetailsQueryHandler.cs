using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Languages.Queries.Shared;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.Languages.Queries.GetDetails;

public class GetLanguageDetailsQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetLanguageDetailsQuery, LanguageVm> {

	public async Task<LanguageVm> Handle(GetLanguageDetailsQuery request, CancellationToken cancellationToken) {
		var vm = await context.Languages
			.AsNoTracking()
			.ProjectTo<LanguageVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(Language), request.Id);

		return vm;
	}
}

