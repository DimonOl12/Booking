using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.HotelReviews.Queries.Shared;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.HotelReviews.Queries.GetDetails;

public class GetHotelReviewDetalisQueryHandler(
	IEasyStayDbContext context,
	IMapper mapper
) : IRequestHandler<GetHotelReviewDetalisQuery, HotelReviewVm> {

	public async Task<HotelReviewVm> Handle(GetHotelReviewDetalisQuery request, CancellationToken cancellationToken) {
		var vm = await context.HotelReviews
			.ProjectTo<HotelReviewVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(hr => hr.Id == request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(HotelReview), request.Id);

		return vm;
	}
}
