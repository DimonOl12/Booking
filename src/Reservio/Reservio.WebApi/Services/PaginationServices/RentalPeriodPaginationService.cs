using AutoMapper;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.RentalPeriods.Queries.GetPage;
using Reservio.Application.MediatR.RentalPeriods.Queries.Shared;
using Reservio.Domain.Entities;

namespace Reservio.WebApi.Services.PaginationServices;

public class RentalPeriodPaginationService(
	IReservioDbContext context,
	IMapper mapper
) : BasePaginationService<RentalPeriod, RentalPeriodVm, GetRentalPeriodsPageQuery>(mapper) {

	protected override IQueryable<RentalPeriod> GetQuery() => context.RentalPeriods.OrderBy(rp => rp.Id);

	protected override IQueryable<RentalPeriod> FilterQueryBeforeProjectTo(IQueryable<RentalPeriod> query, GetRentalPeriodsPageQuery filter) {
		if (filter.Name is not null)
			query = query.Where(rp => rp.Name.ToLower().Contains(filter.Name.ToLower()));

		return query;
	}
}

