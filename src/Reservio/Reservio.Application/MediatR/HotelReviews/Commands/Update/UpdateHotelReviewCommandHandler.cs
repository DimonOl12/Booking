using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.HotelReviews.Commands.Update;

public class UpdateHotelReviewCommandHandler(
	IReservioDbContext context,
	ICurrentUserService currentUserService
) : IRequestHandler<UpdateHotelReviewCommand> {

	public async Task Handle(UpdateHotelReviewCommand request, CancellationToken cancellationToken) {
		var entity = await context.HotelReviews
			.FirstOrDefaultAsync(
				hr => hr.Id == request.Id && hr.Booking.CustomerId == currentUserService.GetRequiredUserId(),
				cancellationToken
			)
			?? throw new NotFoundException(nameof(HotelReview), request.Id);

		entity.Description = request.Description;
		entity.Score = request.Score;
		entity.UpdatedAtUtc = DateTime.UtcNow;

		await context.SaveChangesAsync(cancellationToken);
	}
}
