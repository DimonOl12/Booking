using MediatR;

namespace Reservio.Application.MediatR.FavoriteHotels.Commands.Create;

public class CreateFavoriteHotelCommand : IRequest {
	public long HotelId { get; set; }
}

