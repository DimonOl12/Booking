using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.HotelReviews.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.HotelReviews.Queries.GetPage;

public class GetHotelReviewsPageQueryHandler(
	IPaginationService<HotelReviewVm, GetHotelReviewsPageQuery> pagination
) : IRequestHandler<GetHotelReviewsPageQuery, PageVm<HotelReviewVm>> {

	public Task<PageVm<HotelReviewVm>> Handle(GetHotelReviewsPageQuery request, CancellationToken cancellationToken) =>
		pagination.GetPageAsync(request, cancellationToken);
}

