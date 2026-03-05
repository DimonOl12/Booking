using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.RoomTypes.Commands.Update;

public class UpdateRoomTypeCommandHandler(
	IReservioDbContext context
) : IRequestHandler<UpdateRoomTypeCommand> {

	public async Task Handle(UpdateRoomTypeCommand request, CancellationToken cancellationToken) {
		var entity = await context.RoomTypes.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(RoomType), request.Id);

		entity.Name = request.Name;

		await context.SaveChangesAsync(cancellationToken);
	}
}

