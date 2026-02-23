using AutoMapper;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.HotelCategories.Queries.GetPage;
using EasyStay.Application.MediatR.HotelCategories.Queries.Shared;
using EasyStay.Domain.Entities;

namespace EasyStay.WebApi.Services.PaginationServices;

public class HotelCategoryPaginationService(
	IEasyStayDbContext context,
	IMapper mapper
) : BasePaginationService<HotelCategory, HotelCategoryVm, GetHotelCategoriesPageQuery>(mapper) {

	protected override IQueryable<HotelCategory> GetQuery() => context.HotelCategories.OrderBy(hc => hc.Id);

	protected override IQueryable<HotelCategory> FilterQueryBeforeProjectTo(IQueryable<HotelCategory> query, GetHotelCategoriesPageQuery filter) {
		if (filter.Name is not null)
			query = query.Where(hc => hc.Name.ToLower().Contains(filter.Name.ToLower()));

		return query;
	}
}
