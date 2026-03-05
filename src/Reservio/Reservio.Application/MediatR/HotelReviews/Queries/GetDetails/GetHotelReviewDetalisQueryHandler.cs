using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.HotelReviews.Queries.Shared;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.HotelReviews.Queries.GetDetails;

public class GetHotelReviewDetalisQueryHandler(
	IReservioDbContext context,
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

