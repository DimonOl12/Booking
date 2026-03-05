using MediatR;

namespace Reservio.Application.MediatR.FavoriteHotels.Commands.Delete;

public class DeleteFavoriteHotelCommand : IRequest {
	public long HotelId { get; set; }
}

