using MediatR;

namespace Reservio.Application.MediatR.RoomAmenities.Commands.Create;

public class CreateRoomAmenityCommand : IRequest<long> {
	public string Name { get; set; } = null!;
}

