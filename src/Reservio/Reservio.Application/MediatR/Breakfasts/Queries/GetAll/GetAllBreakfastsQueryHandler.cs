using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Breakfasts.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.Breakfasts.Queries.GetAll;

public class GetAllBreakfastsQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllBreakfastsQuery, IEnumerable<BreakfastVm>> {

	public async Task<IEnumerable<BreakfastVm>> Handle(GetAllBreakfastsQuery request, CancellationToken cancellationToken) {
		var items = await context.Breakfasts
			.AsNoTracking()
			.ProjectTo<BreakfastVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}

