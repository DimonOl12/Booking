using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.RealtorReviews.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.RealtorReviews.Queries.GetPage;

public class GetRealtorReviewsPageQueryHandler(
	IPaginationService<RealtorReviewVm, GetRealtorReviewsPageQuery> pagination
) : IRequestHandler<GetRealtorReviewsPageQuery, PageVm<RealtorReviewVm>> {

	public Task<PageVm<RealtorReviewVm>> Handle(GetRealtorReviewsPageQuery request, CancellationToken cancellationToken) =>
		pagination.GetPageAsync(request);
}

