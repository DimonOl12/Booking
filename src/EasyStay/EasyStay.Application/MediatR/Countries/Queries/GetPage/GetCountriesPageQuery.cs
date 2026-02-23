using EasyStay.Application.MediatR.Countries.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.Countries.Queries.GetPage;

public class GetCountriesPageQuery : PaginationFilterDto, IRequest<PageVm<CountryVm>> {
	public string? Name { get; set; }
}
