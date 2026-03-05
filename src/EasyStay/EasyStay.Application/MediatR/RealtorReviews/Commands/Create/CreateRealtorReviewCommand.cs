using MediatR;

namespace EasyStay.Application.MediatR.RealtorReviews.Commands.Create;

public class CreateRealtorReviewCommand : IRequest<long> {
	public string Description { get; set; } = null!;

	public double? Score { get; set; }

	public long RealtorId { get; set; }
}
