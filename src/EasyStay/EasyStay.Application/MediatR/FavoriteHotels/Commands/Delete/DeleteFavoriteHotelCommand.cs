using MediatR;

namespace EasyStay.Application.MediatR.FavoriteHotels.Commands.Delete;

public class DeleteFavoriteHotelCommand : IRequest {
	public long HotelId { get; set; }
}
