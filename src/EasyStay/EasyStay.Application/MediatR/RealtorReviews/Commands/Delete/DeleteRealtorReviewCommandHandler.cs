using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.RealtorReviews.Commands.Delete;

public class DeleteRealtorReviewCommandHandler(
	IEasyStayDbContext context,
	ICurrentUserService currentUserService
) : IRequestHandler<DeleteRealtorReviewCommand> {

	public async Task Handle(DeleteRealtorReviewCommand request, CancellationToken cancellationToken) {
		var entity = await context.RealtorReviews
			.FirstOrDefaultAsync(
				r => r.Id == request.Id && r.AuthorId == currentUserService.GetRequiredUserId(),
				cancellationToken
			)
			?? throw new NotFoundException(nameof(RealtorReview), request.Id);

		context.RealtorReviews.Remove(entity);
		await context.SaveChangesAsync(cancellationToken);
	}
}