using AutoMapper;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Languages.Queries.GetPage;
using EasyStay.Application.MediatR.Languages.Queries.Shared;
using EasyStay.Domain.Entities;

namespace EasyStay.WebApi.Services.PaginationServices;

public class LanguagePaginationService(
	IEasyStayDbContext context,
	IMapper mapper
) : BasePaginationService<Language, LanguageVm, GetLanguagesPageQuery>(mapper) {

	protected override IQueryable<Language> GetQuery() => context.Languages.OrderBy(l => l.Id);

	protected override IQueryable<Language> FilterQueryBeforeProjectTo(IQueryable<Language> query, GetLanguagesPageQuery filter) {
		if (filter.Name is not null)
			query = query.Where(l => l.Name.ToLower().Contains(filter.Name.ToLower()));

		return query;
	}
}
