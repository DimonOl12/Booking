using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.GuestInfos.Commands.Update;

public class UpdateGuestInfoCommandHandler(
	IReservioDbContext context,
	ICurrentUserService currentUserService
) : IRequestHandler<UpdateGuestInfoCommand> {
	public async Task Handle(UpdateGuestInfoCommand request, CancellationToken cancellationToken) {
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

		entity.AdultCount = request.AdultCount;
		entity.ChildCount = request.ChildCount;

		await context.SaveChangesAsync(cancellationToken);
	}
}

