using AutoMapper;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Languages.Queries.GetPage;
using Reservio.Application.MediatR.Languages.Queries.Shared;
using Reservio.Domain.Entities;

namespace Reservio.WebApi.Services.PaginationServices;

public class LanguagePaginationService(
	IReservioDbContext context,
	IMapper mapper
) : BasePaginationService<Language, LanguageVm, GetLanguagesPageQuery>(mapper) {

	protected override IQueryable<Language> GetQuery() => context.Languages.OrderBy(l => l.Id);

	protected override IQueryable<Language> FilterQueryBeforeProjectTo(IQueryable<Language> query, GetLanguagesPageQuery filter) {
		if (filter.Name is not null)
			query = query.Where(l => l.Name.ToLower().Contains(filter.Name.ToLower()));

		return query;
	}
}

