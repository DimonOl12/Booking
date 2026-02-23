using MediatR;

namespace EasyStay.Application.MediatR.RoomTypes.Commands.Update;

public class UpdateRoomTypeCommand : IRequest {
	public long Id { get; set; }

	public string Name { get; set; } = null!;
}
