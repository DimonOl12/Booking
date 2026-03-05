using AutoMapper;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Citizenships.Queries.GetPage;
using Reservio.Application.MediatR.Citizenships.Queries.Shared;
using Reservio.Domain.Entities;

namespace Reservio.WebApi.Services.PaginationServices;

public class CitizenshipPaginationService(
	IReservioDbContext context,
	IMapper mapper
) : BasePaginationService<Citizenship, CitizenshipVm, GetCitizenshipsPageQuery>(mapper) {

	protected override IQueryable<Citizenship> GetQuery() => context.Citizenships.OrderBy(c => c.Id);

	protected override IQueryable<Citizenship> FilterQueryBeforeProjectTo(IQueryable<Citizenship> query, GetCitizenshipsPageQuery filter) {
		if (filter.Name is not null)
			query = query.Where(c => c.Name.ToLower().Contains(filter.Name.ToLower()));

		return query;
	}
}

