using MediatR;

namespace EasyStay.Application.MediatR.GuestInfos.Commands.Update;

public class UpdateGuestInfoCommand : IRequest {
	public long RoomVariantId { get; set; }

	public int AdultCount { get; set; }

	public int ChildCount { get; set; }
}
