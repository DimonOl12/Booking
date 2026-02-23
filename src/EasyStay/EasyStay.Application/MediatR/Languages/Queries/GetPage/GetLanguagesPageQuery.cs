using EasyStay.Application.MediatR.Languages.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.Languages.Queries.GetPage;

public class GetLanguagesPageQuery : PaginationFilterDto, IRequest<PageVm<LanguageVm>> {
	public string? Name { get; set; }
}
