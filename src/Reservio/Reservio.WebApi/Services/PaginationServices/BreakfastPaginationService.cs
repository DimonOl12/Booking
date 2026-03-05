using AutoMapper;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Breakfasts.Queries.GetPage;
using Reservio.Application.MediatR.Breakfasts.Queries.Shared;
using Reservio.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Reservio.WebApi.Services.PaginationServices;

public class BreakfastPaginationService(
	IReservioDbContext context,
	IMapper mapper
) : BasePaginationService<Breakfast, BreakfastVm, GetBreakfastsPageQuery>(mapper) {

	protected override IQueryable<Breakfast> GetQuery() => context.Breakfasts.AsNoTracking().OrderBy(b => b.Id);

	protected override IQueryable<Breakfast> FilterQueryBeforeProjectTo(IQueryable<Breakfast> query, GetBreakfastsPageQuery filter) {
		if (filter.Name is not null)
			query = query.Where(b => b.Name.ToLower().Contains(filter.Name.ToLower()));

		return query;
	}
}

