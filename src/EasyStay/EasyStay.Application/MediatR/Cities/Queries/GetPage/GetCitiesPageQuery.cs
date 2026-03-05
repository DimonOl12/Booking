using EasyStay.Application.MediatR.Cities.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.Cities.Queries.GetPage;

public class GetCitiesPageQuery : PaginationFilterDto, IRequest<PageVm<CityVm>> {
	public string? Name { get; set; }

	public double? Longitude { get; set; }
	public double? Latitude { get; set; }

	public double? MinLongitude { get; set; }
	public double? MaxLongitude { get; set; }
	public double? MinLatitude { get; set; }
	public double? MaxLatitude { get; set; }

	public long? CountryId { get; set; }

	public bool? IsRandomItems { get; set; }
}
