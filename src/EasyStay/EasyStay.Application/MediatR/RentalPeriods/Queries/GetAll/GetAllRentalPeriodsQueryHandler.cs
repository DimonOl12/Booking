using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.RentalPeriods.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.RentalPeriods.Queries.GetAll;

public class GetAllRentalPeriodsQueryHandler(
	IEasyStayDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllRentalPeriodsQuery, IEnumerable<RentalPeriodVm>> {

	public async Task<IEnumerable<RentalPeriodVm>> Handle(GetAllRentalPeriodsQuery request, CancellationToken cancellationToken) {
		var items = await context.RentalPeriods
			.ProjectTo<RentalPeriodVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}
