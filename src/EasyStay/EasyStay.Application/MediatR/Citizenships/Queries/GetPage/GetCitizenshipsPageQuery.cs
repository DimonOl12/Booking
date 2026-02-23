using EasyStay.Application.MediatR.Citizenships.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.Citizenships.Queries.GetPage;

public class GetCitizenshipsPageQuery : PaginationFilterDto, IRequest<PageVm<CitizenshipVm>> {
	public string? Name { get; set; }
}
