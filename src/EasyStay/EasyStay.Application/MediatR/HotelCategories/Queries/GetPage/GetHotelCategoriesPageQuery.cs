using EasyStay.Application.MediatR.HotelCategories.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.HotelCategories.Queries.GetPage;

public class GetHotelCategoriesPageQuery : PaginationFilterDto, IRequest<PageVm<HotelCategoryVm>> {
	public string? Name { get; set; }
}
