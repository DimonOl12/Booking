using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.RoomAmenities.Commands.Delete;

public class DeleteRoomAmenityCommandHandle(
	IReservioDbContext context
) : IRequestHandler<DeleteRoomAmenityCommand> {

	public async Task Handle(DeleteRoomAmenityCommand request, CancellationToken cancellationToken) {
		var enity = await context.RoomAmenities.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(RoomAmenity), request.Id);

		context.RoomAmenities.Remove(enity);
		await context.SaveChangesAsync(cancellationToken);
	}
}

