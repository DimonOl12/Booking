using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.Cities.Queries.GetAdvertisingPage;

public class GetCitiesAdvertisingPageQuery : PaginationFilterDto, IRequest<PageVm<CityAdvertisingVm>> {
	public bool? HasMinPrice { get; set; }
}

