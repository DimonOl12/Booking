using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using Reservio.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.RealtorReviews.Commands.Create;

public class CreateRealtorReviewCommandHandler(
	IReservioDbContext context,
	ICurrentUserService currentUserService,
	UserManager<User> userManager
) : IRequestHandler<CreateRealtorReviewCommand, long> {

	public async Task<long> Handle(CreateRealtorReviewCommand request, CancellationToken cancellationToken) {
		var realtor = await userManager.Users
			.OfType<Realtor>()
			.FirstOrDefaultAsync(r => r.Id == request.RealtorId, cancellationToken)
			?? throw new NotFoundException(nameof(Realtor), request.RealtorId);

		var entity = new RealtorReview {
			Description = request.Description,
			Score = request.Score,
			CreatedAtUtc = DateTime.UtcNow,
			UpdatedAtUtc = null,
			AuthorId = currentUserService.GetRequiredUserId(),
			RealtorId = request.RealtorId
		};

		await context.RealtorReviews.AddAsync(entity, cancellationToken);
		await context.SaveChangesAsync(cancellationToken);

		return entity.Id;
	}
}

