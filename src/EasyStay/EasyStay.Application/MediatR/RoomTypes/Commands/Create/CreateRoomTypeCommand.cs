using MediatR;

namespace EasyStay.Application.MediatR.RoomTypes.Commands.Create;

public class CreateRoomTypeCommand : IRequest<long> {
	public string Name { get; set; } = null!;
}
