using MediatR;

namespace EasyStay.Application.MediatR.HotelReviews.Commands.Update;

public class UpdateHotelReviewCommand : IRequest {
	public long Id { get; set; }

	public string Description { get; set; } = null!;

	public double? Score { get; set; }
}
