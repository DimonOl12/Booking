using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.Cities.Queries.GetAdvertisingPage;

public class GetCitiesAdvertisingPageQuery : PaginationFilterDto, IRequest<PageVm<CityAdvertisingVm>> {
	public bool? HasMinPrice { get; set; }
}
