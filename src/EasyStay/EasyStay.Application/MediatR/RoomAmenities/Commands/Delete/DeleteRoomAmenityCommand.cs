using MediatR;

namespace EasyStay.Application.MediatR.RoomAmenities.Commands.Delete;

public class DeleteRoomAmenityCommand : IRequest {
	public long Id { get; set; }
}
