using MediatR;

namespace EasyStay.Application.MediatR.HotelReviews.Commands.Delete;

public class DeleteHotelReviewCommand : IRequest {
	public long Id { get; set; }
}
