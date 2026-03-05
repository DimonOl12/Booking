using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.HotelCategories.Queries.Shared;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.HotelCategories.Queries.GetDetails;

public class GetHotelCategoryDetailsQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetHotelCategoryDetailsQuery, HotelCategoryVm> {

	public async Task<HotelCategoryVm> Handle(GetHotelCategoryDetailsQuery request, CancellationToken cancellationToken) {
		var vm = await context.HotelCategories
			.ProjectTo<HotelCategoryVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(ht => ht.Id == request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(HotelCategory), request.Id);

		return vm;
	}
}

