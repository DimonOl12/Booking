using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Languages.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.Languages.Queries.GetPage;

public class GetLanguagesPageQueryHandler(
	IPaginationService<LanguageVm, GetLanguagesPageQuery> pagination
) : IRequestHandler<GetLanguagesPageQuery, PageVm<LanguageVm>> {

	public Task<PageVm<LanguageVm>> Handle(GetLanguagesPageQuery request, CancellationToken cancellationToken)
		=> pagination.GetPageAsync(request, cancellationToken);
}

