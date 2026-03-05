using Reservio.Application.MediatR.HotelReviews.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.HotelReviews.Queries.GetDetails;

public class GetHotelReviewDetalisQuery : IRequest<HotelReviewVm> {
	public long Id { get; set; }
}

