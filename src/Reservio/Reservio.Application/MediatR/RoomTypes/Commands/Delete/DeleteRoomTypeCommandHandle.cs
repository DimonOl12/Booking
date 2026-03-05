using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.RoomTypes.Commands.Delete;

public class DeleteRoomTypeCommandHandle(
	IReservioDbContext context
) : IRequestHandler<DeleteRoomTypeCommand> {

	public async Task Handle(DeleteRoomTypeCommand request, CancellationToken cancellationToken) {
		var enity = await context.RoomTypes.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(RoomType), request.Id);

		context.RoomTypes.Remove(enity);
		await context.SaveChangesAsync(cancellationToken);
	}
}

