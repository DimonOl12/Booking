using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.RentalPeriods.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.RentalPeriods.Queries.GetAll;

public class GetAllRentalPeriodsQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllRentalPeriodsQuery, IEnumerable<RentalPeriodVm>> {

	public async Task<IEnumerable<RentalPeriodVm>> Handle(GetAllRentalPeriodsQuery request, CancellationToken cancellationToken) {
		var items = await context.RentalPeriods
			.ProjectTo<RentalPeriodVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}

