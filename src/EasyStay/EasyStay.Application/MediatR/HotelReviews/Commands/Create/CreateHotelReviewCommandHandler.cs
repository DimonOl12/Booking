using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.HotelReviews.Commands.Create;

public class CreateHotelReviewCommandHandler(
	IEasyStayDbContext context
) : IRequestHandler<CreateHotelReviewCommand, long> {

	public async Task<long> Handle(CreateHotelReviewCommand request, CancellationToken cancellationToken) {
		var entity = new HotelReview {
			Description = request.Description,
			Score = request.Score,
			CreatedAtUtc = DateTime.UtcNow,
			UpdatedAtUtc = null,
			BookingId = request.BookingId,
		};

		await context.HotelReviews.AddAsync(entity, cancellationToken);
		await context.SaveChangesAsync(cancellationToken);

		return entity.Id;
	}
}
