using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Breakfasts.Queries.Shared;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Breakfasts.Queries.GetDetails;

public class GetBreakfastDetailsQueryHandler(
	IEasyStayDbContext context,
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
