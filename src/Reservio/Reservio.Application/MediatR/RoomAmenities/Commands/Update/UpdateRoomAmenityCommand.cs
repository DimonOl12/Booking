using MediatR;

namespace Reservio.Application.MediatR.RoomAmenities.Commands.Update;

public class UpdateRoomAmenityCommand : IRequest {
	public long Id { get; set; }

	public string Name { get; set; } = null!;
}

