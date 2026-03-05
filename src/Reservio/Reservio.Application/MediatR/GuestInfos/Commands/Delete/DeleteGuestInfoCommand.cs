using MediatR;

namespace Reservio.Application.MediatR.GuestInfos.Commands.Delete;

public class DeleteGuestInfoCommand : IRequest {
	public long RoomVariantId { get; set; }
}

