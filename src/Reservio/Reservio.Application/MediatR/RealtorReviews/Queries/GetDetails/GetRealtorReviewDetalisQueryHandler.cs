using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.RealtorReviews.Queries.Shared;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.RealtorReviews.Queries.GetDetails;

public class GetRealtorReviewDetalisQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetRealtorReviewDetalisQuery, RealtorReviewVm> {

	public async Task<RealtorReviewVm> Handle(GetRealtorReviewDetalisQuery request, CancellationToken cancellationToken) {
		var vm = await context.RealtorReviews
			.ProjectTo<RealtorReviewVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(rr => rr.Id == request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(RealtorReview), request.Id);

		return vm;
	}
}

