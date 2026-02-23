using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.BedInfos.Commands.Delete;

public class DeleteBedInfoCommandHandler(
	IEasyStayDbContext context,
	ICurrentUserService currentUserService
) : IRequestHandler<DeleteBedInfoCommand> {

	public async Task Handle(DeleteBedInfoCommand request, CancellationToken cancellationToken) {
		var entity = await context.BedInfos
			.Include(bi => bi.RoomVariant)
				.ThenInclude(rv => rv.Room)
					.ThenInclude(r => r.Hotel)
			.FirstOrDefaultAsync(
				bi => bi.RoomVariantId == request.RoomVariantId
					&& bi.RoomVariant.Room.Hotel.RealtorId == currentUserService.GetRequiredUserId(),
				cancellationToken
			)
			?? throw new NotFoundException(nameof(BedInfo), request.RoomVariantId);

		context.BedInfos.Remove(entity);
		await context.SaveChangesAsync(cancellationToken);
	}
}
