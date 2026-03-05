using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.Hotels.Queries.GetDetails;

public class GetHotelDetailsQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetHotelDetailsQuery, HotelDetailsVm> {

	public async Task<HotelDetailsVm> Handle(GetHotelDetailsQuery request, CancellationToken cancellationToken) {
		var vm = await context.Hotels
			.AsNoTracking()
			.ProjectTo<HotelDetailsVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(h => h.Id == request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(Hotel), request.Id);

		return vm;
	}
}

