using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.GuestInfos.Commands.Delete;

public class DeleteGuestInfoCommandHandler(
	IReservioDbContext context,
	ICurrentUserService currentUserService
) : IRequestHandler<DeleteGuestInfoCommand> {

	public async Task Handle(DeleteGuestInfoCommand request, CancellationToken cancellationToken) {
		var entity = await context.GuestInfos
			.Include(gi => gi.RoomVariant)
				.ThenInclude(rv => rv.Room)
					.ThenInclude(r => r.Hotel)
			.FirstOrDefaultAsync(
				gi => gi.RoomVariantId == request.RoomVariantId
					&& gi.RoomVariant.Room.Hotel.RealtorId == currentUserService.GetRequiredUserId(),
				cancellationToken
			)
			?? throw new NotFoundException(nameof(GuestInfo), request.RoomVariantId);

		context.GuestInfos.Remove(entity);
		await context.SaveChangesAsync(cancellationToken);
	}
}

