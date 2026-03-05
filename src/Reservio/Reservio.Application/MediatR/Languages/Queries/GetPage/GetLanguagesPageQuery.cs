using Reservio.Application.MediatR.Languages.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.Languages.Queries.GetPage;

public class GetLanguagesPageQuery : PaginationFilterDto, IRequest<PageVm<LanguageVm>> {
	public string? Name { get; set; }
}

