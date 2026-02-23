using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.HotelReviews.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.HotelReviews.Queries.GetPage;

public class GetHotelReviewsPageQueryHandler(
	IPaginationService<HotelReviewVm, GetHotelReviewsPageQuery> pagination
) : IRequestHandler<GetHotelReviewsPageQuery, PageVm<HotelReviewVm>> {

	public Task<PageVm<HotelReviewVm>> Handle(GetHotelReviewsPageQuery request, CancellationToken cancellationToken) =>
		pagination.GetPageAsync(request, cancellationToken);
}
