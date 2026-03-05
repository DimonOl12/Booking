using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.HotelAmenities.Queries.Shared;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.HotelAmenities.Queries.GetDetails;

public class GetHotelAmenityDetailsQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetHotelAmenityDetailsQuery, HotelAmenityVm> {

	public async Task<HotelAmenityVm> Handle(GetHotelAmenityDetailsQuery request, CancellationToken cancellationToken) {
		var vm = await context.HotelAmenities
			.ProjectTo<HotelAmenityVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(ha => ha.Id == request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(HotelAmenity), request.Id);

		return vm;
	}
}

