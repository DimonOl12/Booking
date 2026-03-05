using AutoMapper;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Breakfasts.Queries.GetPage;
using EasyStay.Application.MediatR.Breakfasts.Queries.Shared;
using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.WebApi.Services.PaginationServices;

public class BreakfastPaginationService(
	IEasyStayDbContext context,
	IMapper mapper
) : BasePaginationService<Breakfast, BreakfastVm, GetBreakfastsPageQuery>(mapper) {

	protected override IQueryable<Breakfast> GetQuery() => context.Breakfasts.AsNoTracking().OrderBy(b => b.Id);

	protected override IQueryable<Breakfast> FilterQueryBeforeProjectTo(IQueryable<Breakfast> query, GetBreakfastsPageQuery filter) {
		if (filter.Name is not null)
			query = query.Where(b => b.Name.ToLower().Contains(filter.Name.ToLower()));

		return query;
	}
}
