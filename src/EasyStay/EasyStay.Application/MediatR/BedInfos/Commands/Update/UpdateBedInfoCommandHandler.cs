using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.BedInfos.Commands.Update;

public class UpdateBedInfoCommandHandler(
	IEasyStayDbContext context,
	ICurrentUserService currentUserService
) : IRequestHandler<UpdateBedInfoCommand> {

	public async Task Handle(UpdateBedInfoCommand request, CancellationToken cancellationToken) {
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

		entity.SingleBedCount = request.SingleBedCount;
		entity.DoubleBedCount = request.DoubleBedCount;
		entity.ExtraBedCount = request.ExtraBedCount;
		entity.SofaCount = request.SofaCount;
		entity.KingsizeBedCount = request.KingsizeBedCount;

		await context.SaveChangesAsync(cancellationToken);
	}
}
