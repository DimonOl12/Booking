using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.BedInfos.Commands.Create;

public class CreateBedInfoCommandHandler(
	IReservioDbContext context
) : IRequestHandler<CreateBedInfoCommand, long> {

	public async Task<long> Handle(CreateBedInfoCommand request, CancellationToken cancellationToken) {
		var entity = new BedInfo {
			RoomVariantId = request.RoomVariantId,
			SingleBedCount = request.SingleBedCount,
			DoubleBedCount = request.DoubleBedCount,
			ExtraBedCount = request.ExtraBedCount,
			SofaCount = request.SofaCount,
			KingsizeBedCount = request.KingsizeBedCount
		};

		await context.BedInfos.AddAsync(entity, cancellationToken);

		await context.SaveChangesAsync(cancellationToken);

		return entity.RoomVariantId;
	}
}

