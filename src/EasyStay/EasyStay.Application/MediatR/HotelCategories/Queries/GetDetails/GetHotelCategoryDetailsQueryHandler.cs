using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.HotelCategories.Queries.Shared;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.HotelCategories.Queries.GetDetails;

public class GetHotelCategoryDetailsQueryHandler(
	IEasyStayDbContext context,
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
