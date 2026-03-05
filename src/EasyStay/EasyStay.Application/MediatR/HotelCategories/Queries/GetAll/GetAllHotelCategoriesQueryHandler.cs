using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.HotelCategories.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.HotelCategories.Queries.GetAll;

public class GetAllHotelCategoriesQueryHandler(
	IEasyStayDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllHotelCategoriesQuery, IEnumerable<HotelCategoryVm>> {

	public async Task<IEnumerable<HotelCategoryVm>> Handle(GetAllHotelCategoriesQuery request, CancellationToken cancellationToken) {
		var items = await context.HotelCategories
			.ProjectTo<HotelCategoryVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}
