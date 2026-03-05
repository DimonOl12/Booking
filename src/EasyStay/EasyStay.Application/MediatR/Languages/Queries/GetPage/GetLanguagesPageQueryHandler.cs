using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Languages.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.Languages.Queries.GetPage;

public class GetLanguagesPageQueryHandler(
	IPaginationService<LanguageVm, GetLanguagesPageQuery> pagination
) : IRequestHandler<GetLanguagesPageQuery, PageVm<LanguageVm>> {

	public Task<PageVm<LanguageVm>> Handle(GetLanguagesPageQuery request, CancellationToken cancellationToken)
		=> pagination.GetPageAsync(request, cancellationToken);
}
