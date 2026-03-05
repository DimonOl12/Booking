using MediatR;

namespace EasyStay.Application.MediatR.RoomAmenities.Commands.Create;

public class CreateRoomAmenityCommand : IRequest<long> {
	public string Name { get; set; } = null!;
}
