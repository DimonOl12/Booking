using AutoMapper;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.RentalPeriods.Queries.GetPage;
using EasyStay.Application.MediatR.RentalPeriods.Queries.Shared;
using EasyStay.Domain.Entities;

namespace EasyStay.WebApi.Services.PaginationServices;

public class RentalPeriodPaginationService(
	IEasyStayDbContext context,
	IMapper mapper
) : BasePaginationService<RentalPeriod, RentalPeriodVm, GetRentalPeriodsPageQuery>(mapper) {

	protected override IQueryable<RentalPeriod> GetQuery() => context.RentalPeriods.OrderBy(rp => rp.Id);

	protected override IQueryable<RentalPeriod> FilterQueryBeforeProjectTo(IQueryable<RentalPeriod> query, GetRentalPeriodsPageQuery filter) {
		if (filter.Name is not null)
			query = query.Where(rp => rp.Name.ToLower().Contains(filter.Name.ToLower()));

		return query;
	}
}
