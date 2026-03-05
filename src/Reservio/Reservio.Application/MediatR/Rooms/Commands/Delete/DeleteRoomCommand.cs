using MediatR;

namespace Reservio.Application.MediatR.Rooms.Commands.Delete;

public class DeleteRoomCommand : IRequest {
	public long Id { get; set; }
}

