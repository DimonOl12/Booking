using MediatR;

namespace EasyStay.Application.MediatR.RealtorReviews.Commands.Update;

public class UpdateRealtorReviewCommand : IRequest {
	public long Id { get; set; }

	public string Description { get; set; } = null!;

	public double? Score { get; set; }
}
