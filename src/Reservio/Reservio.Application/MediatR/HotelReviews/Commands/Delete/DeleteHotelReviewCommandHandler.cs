using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.HotelReviews.Commands.Delete;

public class DeleteHotelReviewCommandHandler(
	IReservioDbContext context,
	ICurrentUserService currentUserService
) : IRequestHandler<DeleteHotelReviewCommand> {

	public async Task Handle(DeleteHotelReviewCommand request, CancellationToken cancellationToken) {
		var entity = await context.HotelReviews
			.FirstOrDefaultAsync(
				hr => hr.Id == request.Id && hr.Booking.CustomerId == currentUserService.GetRequiredUserId(),
				cancellationToken
			)
			?? throw new NotFoundException(nameof(HotelReview), request.Id);

		context.HotelReviews.Remove(entity);
		await context.SaveChangesAsync(cancellationToken);
	}
}

