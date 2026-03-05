using MediatR;

namespace Reservio.Application.MediatR.GuestInfos.Commands.Create;

public class CreateGuestInfoCommand : IRequest<long> {
	public long RoomVariantId { get; set; }

	public int AdultCount { get; set; }

	public int ChildCount { get; set; }
}

