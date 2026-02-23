using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Breakfasts.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Breakfasts.Queries.GetAll;

public class GetAllBreakfastsQueryHandler(
	IEasyStayDbContext context,
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
