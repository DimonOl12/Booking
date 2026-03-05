using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.HotelCategories.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.HotelCategories.Queries.GetAll;

public class GetAllHotelCategoriesQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllHotelCategoriesQuery, IEnumerable<HotelCategoryVm>> {

	public async Task<IEnumerable<HotelCategoryVm>> Handle(GetAllHotelCategoriesQuery request, CancellationToken cancellationToken) {
		var items = await context.HotelCategories
			.ProjectTo<HotelCategoryVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}

