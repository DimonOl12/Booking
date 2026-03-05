using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.FavoriteHotels.Commands.Delete;

public class DeleteFavoriteHotelCommandHandler(
	IReservioDbContext context,
	ICurrentUserService currentUserService
) : IRequestHandler<DeleteFavoriteHotelCommand> {

	public async Task Handle(DeleteFavoriteHotelCommand request, CancellationToken cancellationToken) {
		var countOfDeleted = await context.FavoriteHotels
			.Where(fh => fh.HotelId == request.HotelId && fh.CustomerId == currentUserService.GetRequiredUserId())
			.ExecuteDeleteAsync(cancellationToken);

		if (countOfDeleted == 0)
			throw new NotFoundException(nameof(Hotel), request.HotelId);
	}
}

