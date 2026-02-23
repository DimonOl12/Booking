using MediatR;

namespace EasyStay.Application.MediatR.BedInfos.Commands.Delete;

public class DeleteBedInfoCommand : IRequest {
	public long RoomVariantId { get; set; }
}
