using AutoMapper;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.HotelCategories.Queries.GetPage;
using Reservio.Application.MediatR.HotelCategories.Queries.Shared;
using Reservio.Domain.Entities;

namespace Reservio.WebApi.Services.PaginationServices;

public class HotelCategoryPaginationService(
	IReservioDbContext context,
	IMapper mapper
) : BasePaginationService<HotelCategory, HotelCategoryVm, GetHotelCategoriesPageQuery>(mapper) {

	protected override IQueryable<HotelCategory> GetQuery() => context.HotelCategories.OrderBy(hc => hc.Id);

	protected override IQueryable<HotelCategory> FilterQueryBeforeProjectTo(IQueryable<HotelCategory> query, GetHotelCategoriesPageQuery filter) {
		if (filter.Name is not null)
			query = query.Where(hc => hc.Name.ToLower().Contains(filter.Name.ToLower()));

		return query;
	}
}

