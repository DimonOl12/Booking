using Reservio.Application.MediatR.Citizenships.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.Citizenships.Queries.GetPage;

public class GetCitizenshipsPageQuery : PaginationFilterDto, IRequest<PageVm<CitizenshipVm>> {
	public string? Name { get; set; }
}

