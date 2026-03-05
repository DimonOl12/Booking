using Reservio.Application.MediatR.HotelCategories.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.HotelCategories.Queries.GetPage;

public class GetHotelCategoriesPageQuery : PaginationFilterDto, IRequest<PageVm<HotelCategoryVm>> {
	public string? Name { get; set; }
}

