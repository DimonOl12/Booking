using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Breakfasts.Queries.Shared;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.Breakfasts.Queries.GetDetails;

public class GetBreakfastDetailsQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetBreakfastDetailsQuery, BreakfastVm> {

	public async Task<BreakfastVm> Handle(GetBreakfastDetailsQuery request, CancellationToken cancellationToken) {
		var vm = await context.Breakfasts
			.AsNoTracking()
			.ProjectTo<BreakfastVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(Breakfast), request.Id);

		return vm;
	}
}

