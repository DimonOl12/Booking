using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.RoomAmenities.Commands.Update;

public class UpdateRoomAmenityCommandHandler(
	IReservioDbContext context
) : IRequestHandler<UpdateRoomAmenityCommand> {

	public async Task Handle(UpdateRoomAmenityCommand request, CancellationToken cancellationToken) {
		var entity = await context.RoomAmenities.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(RoomAmenity), request.Id);

		entity.Name = request.Name;

		await context.SaveChangesAsync(cancellationToken);
	}
}

