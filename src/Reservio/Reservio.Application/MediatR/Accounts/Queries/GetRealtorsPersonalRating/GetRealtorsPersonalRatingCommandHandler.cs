using Reservio.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.Accounts.Queries.GetRealtorsPersonalRating;

public class GetRealtorsPersonalRatingCommandHandler(
	IReservioDbContext context,
	ICurrentUserService currentUserService
) : IRequestHandler<GetRealtorsPersonalRatingCommand, double> {

	public Task<double> Handle(GetRealtorsPersonalRatingCommand request, CancellationToken cancellationToken) =>
		context.Realtors
			.Where(r => r.Id == currentUserService.GetRequiredUserId())
			.Select(r => r.Reviews.Average(r => r.Score).GetValueOrDefault(0))
			.FirstAsync(cancellationToken);
}

