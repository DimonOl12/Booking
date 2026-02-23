using EasyStay.Application.MediatR.HotelAmenities.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.HotelAmenities.Queries.GetPage;

public class GetHotelAmenitiesPageQuery : PaginationFilterDto, IRequest<PageVm<HotelAmenityVm>> {
	public string? Name { get; set; }
}
