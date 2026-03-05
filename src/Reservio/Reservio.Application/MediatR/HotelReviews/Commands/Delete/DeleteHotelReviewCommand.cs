using MediatR;

namespace Reservio.Application.MediatR.HotelReviews.Commands.Delete;

public class DeleteHotelReviewCommand : IRequest {
	public long Id { get; set; }
}

