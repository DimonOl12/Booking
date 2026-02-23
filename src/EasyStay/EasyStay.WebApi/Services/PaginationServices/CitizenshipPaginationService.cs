using AutoMapper;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Citizenships.Queries.GetPage;
using EasyStay.Application.MediatR.Citizenships.Queries.Shared;
using EasyStay.Domain.Entities;

namespace EasyStay.WebApi.Services.PaginationServices;

public class CitizenshipPaginationService(
	IEasyStayDbContext context,
	IMapper mapper
) : BasePaginationService<Citizenship, CitizenshipVm, GetCitizenshipsPageQuery>(mapper) {

	protected override IQueryable<Citizenship> GetQuery() => context.Citizenships.OrderBy(c => c.Id);

	protected override IQueryable<Citizenship> FilterQueryBeforeProjectTo(IQueryable<Citizenship> query, GetCitizenshipsPageQuery filter) {
		if (filter.Name is not null)
			query = query.Where(c => c.Name.ToLower().Contains(filter.Name.ToLower()));

		return query;
	}
}
