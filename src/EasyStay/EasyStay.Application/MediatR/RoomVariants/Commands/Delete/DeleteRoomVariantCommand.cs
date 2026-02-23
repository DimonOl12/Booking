using MediatR;

namespace EasyStay.Application.MediatR.RoomVariants.Commands.Delete;

public class DeleteRoomVariantCommand : IRequest {
	public long Id { get; set; }
}
