using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.RealtorReviews.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.RealtorReviews.Queries.GetPage;

public class GetRealtorReviewsPageQueryHandler(
	IPaginationService<RealtorReviewVm, GetRealtorReviewsPageQuery> pagination
) : IRequestHandler<GetRealtorReviewsPageQuery, PageVm<RealtorReviewVm>> {

	public Task<PageVm<RealtorReviewVm>> Handle(GetRealtorReviewsPageQuery request, CancellationToken cancellationToken) =>
		pagination.GetPageAsync(request);
}
