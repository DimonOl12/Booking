using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.HotelAmenities.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.HotelAmenities.Queries.GetAll;

public class GetAllHotelAmenitiesQueryHandler(
	IEasyStayDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllHotelAmenitiesQuery, IEnumerable<HotelAmenityVm>> {

	public async Task<IEnumerable<HotelAmenityVm>> Handle(GetAllHotelAmenitiesQuery request, CancellationToken cancellationToken) {
		var items = await context.HotelAmenities
			.ProjectTo<HotelAmenityVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}
