using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.GuestInfos.Commands.Create;

public class CreateGuestInfoCommandHandler(
	IReservioDbContext context
) : IRequestHandler<CreateGuestInfoCommand, long> {
	public async Task<long> Handle(CreateGuestInfoCommand request, CancellationToken cancellationToken) {
		var entity = new GuestInfo {
			RoomVariantId = request.RoomVariantId,
			AdultCount = request.AdultCount,
			ChildCount = request.ChildCount
		};

		await context.GuestInfos.AddAsync(entity, cancellationToken);

		await context.SaveChangesAsync(cancellationToken);

		return entity.RoomVariantId;
	}
}

