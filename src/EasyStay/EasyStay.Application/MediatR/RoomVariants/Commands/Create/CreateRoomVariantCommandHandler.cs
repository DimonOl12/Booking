using AutoMapper;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.BedInfos.Commands.Create;
using EasyStay.Application.MediatR.GuestInfos.Commands.Create;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.RoomVariants.Commands.Create;

public class CreateRoomVariantCommandHandler(
	IEasyStayDbContext context,
	IMediator mediator,
	IMapper mapper
) : IRequestHandler<CreateRoomVariantCommand, long> {

	public async Task<long> Handle(CreateRoomVariantCommand request, CancellationToken cancellationToken) {
		var entity = new RoomVariant {
			Price = request.Price,
			DiscountPrice = request.DiscountPrice,
			RoomId = request.RoomId
		};

		await context.RoomVariants.AddAsync(entity, cancellationToken);

		using var transaction = await context.BeginTransactionAsync(cancellationToken);
		try {
			await context.SaveChangesAsync(cancellationToken);

			var guestInfoCommand = mapper.Map<CreateGuestInfoCommand>(request.GuestInfo);
			guestInfoCommand.RoomVariantId = entity.Id;
			await mediator.Send(guestInfoCommand, cancellationToken);

			var bedInfoCommand = mapper.Map<CreateBedInfoCommand>(request.BedInfo);
			bedInfoCommand.RoomVariantId = entity.Id;
			await mediator.Send(bedInfoCommand, cancellationToken);

			await transaction.CommitAsync(cancellationToken);
		}
		catch {
			await transaction.RollbackAsync(cancellationToken);
		}

		return entity.Id;
	}
}
