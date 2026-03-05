using MediatR;

namespace EasyStay.Application.MediatR.BedInfos.Commands.Create;

public class CreateBedInfoCommand : IRequest<long> {
	public long RoomVariantId { get; set; }

	public int SingleBedCount { get; set; }

	public int DoubleBedCount { get; set; }

	public int ExtraBedCount { get; set; }

	public int SofaCount { get; set; }

	public int KingsizeBedCount { get; set; }
}
