using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.RentalPeriods.Queries.Shared;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.RentalPeriods.Queries.GetDetails;

public class GetRentalPeriodDetailsQueryHandler(
	IEasyStayDbContext context,
	IMapper mapper
) : IRequestHandler<GetRentalPeriodDetailsQuery, RentalPeriodVm> {

	public async Task<RentalPeriodVm> Handle(GetRentalPeriodDetailsQuery request, CancellationToken cancellationToken) {
		var vm = await context.RentalPeriods
			.ProjectTo<RentalPeriodVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(rp => rp.Id == request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(RentalPeriod), request.Id);

		return vm;
	}
}
