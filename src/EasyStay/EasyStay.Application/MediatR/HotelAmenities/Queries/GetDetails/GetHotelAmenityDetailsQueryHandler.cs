using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.HotelAmenities.Queries.Shared;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.HotelAmenities.Queries.GetDetails;

public class GetHotelAmenityDetailsQueryHandler(
	IEasyStayDbContext context,
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
