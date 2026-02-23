using MediatR;

namespace EasyStay.Application.MediatR.RoomTypes.Commands.Delete;

public class DeleteRoomTypeCommand : IRequest {
	public long Id { get; set; }
}
