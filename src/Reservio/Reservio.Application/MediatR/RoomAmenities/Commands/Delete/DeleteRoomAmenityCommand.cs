using MediatR;

namespace Reservio.Application.MediatR.RoomAmenities.Commands.Delete;

public class DeleteRoomAmenityCommand : IRequest {
	public long Id { get; set; }
}

