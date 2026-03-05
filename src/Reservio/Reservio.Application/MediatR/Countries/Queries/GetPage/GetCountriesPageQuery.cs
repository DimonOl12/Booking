using Reservio.Application.MediatR.Countries.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.Countries.Queries.GetPage;

public class GetCountriesPageQuery : PaginationFilterDto, IRequest<PageVm<CountryVm>> {
	public string? Name { get; set; }
}

