using MediatR;

namespace EasyStay.Application.MediatR.HotelReviews.Commands.Create;

public class CreateHotelReviewCommand : IRequest<long> {
	public string Description { get; set; } = null!;

	public double? Score { get; set; }

	public long BookingId { get; set; }
}
