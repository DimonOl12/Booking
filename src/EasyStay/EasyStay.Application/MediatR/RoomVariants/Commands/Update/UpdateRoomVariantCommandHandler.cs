using AutoMapper;
using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.BedInfos.Commands.Update;
using EasyStay.Application.MediatR.GuestInfos.Commands.Update;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.RoomVariants.Commands.Update;

public class UpdateRoomVariantCommandHandler(
	IEasyStayDbContext context,
	IMediator mediator,
	IMapper mapper,
	ICurrentUserService currentUserService
) : IRequestHandler<UpdateRoomVariantCommand> {

	public async Task Handle(UpdateRoomVariantCommand request, CancellationToken cancellationToken) {
		var entity = await context.RoomVariants
			.Include(rv => rv.Room)
				.ThenInclude(r => r.Hotel)
			.Include(rv => rv.GuestInfo)
			.Include(rv => rv.BedInfo)
			.FirstOrDefaultAsync(
				rv => rv.Id == request.Id && rv.Room.Hotel.RealtorId == currentUserService.GetRequiredUserId(),
				cancellationToken
			)
			?? throw new NotFoundException(nameof(RoomVariant), request.Id);

		entity.Price = request.Price;
		entity.DiscountPrice = request.DiscountPrice;

		using var transaction = await context.BeginTransactionAsync(cancellationToken);
		try {
			await context.SaveChangesAsync(cancellationToken);

			var guestInfoCommand = mapper.Map<UpdateGuestInfoCommand>(request.GuestInfo);
			guestInfoCommand.RoomVariantId = entity.Id;
			await mediator.Send(guestInfoCommand, cancellationToken);

			var bedInfoCommand = mapper.Map<UpdateBedInfoCommand>(request.BedInfo);
			bedInfoCommand.RoomVariantId = entity.Id;
			await mediator.Send(bedInfoCommand, cancellationToken);

			await transaction.CommitAsync(cancellationToken);
		}
		catch {
			await transaction.RollbackAsync(cancellationToken);
		}
	}
}
