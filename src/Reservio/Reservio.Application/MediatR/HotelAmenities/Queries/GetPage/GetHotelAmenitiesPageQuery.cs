using Reservio.Application.MediatR.HotelAmenities.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.HotelAmenities.Queries.GetPage;

public class GetHotelAmenitiesPageQuery : PaginationFilterDto, IRequest<PageVm<HotelAmenityVm>> {
	public string? Name { get; set; }
}

