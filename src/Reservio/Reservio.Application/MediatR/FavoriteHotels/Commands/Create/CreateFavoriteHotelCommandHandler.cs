using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.FavoriteHotels.Commands.Create;

public class CreateFavoriteHotelCommandHandler(
	IReservioDbContext context,
	ICurrentUserService currentUserService
) : IRequestHandler<CreateFavoriteHotelCommand> {

	public async Task Handle(CreateFavoriteHotelCommand request, CancellationToken cancellationToken) {
		var favoriteHotel = new FavoriteHotel {
			HotelId = request.HotelId,
			CustomerId = currentUserService.GetRequiredUserId()
		};

		await context.FavoriteHotels.AddAsync(favoriteHotel, cancellationToken);

		await context.SaveChangesAsync(cancellationToken);
	}
}

