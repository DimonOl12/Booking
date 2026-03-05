using MediatR;

namespace Reservio.Application.MediatR.HotelAmenities.Commands.Delete;

public class DeleteHotelAmenityCommand : IRequest {
	public long Id { get; set; }
}

