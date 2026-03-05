using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.RoomAmenities.Commands.Create;

public class CreateRoomAmenityCommandHandler(
	IReservioDbContext context
) : IRequestHandler<CreateRoomAmenityCommand, long> {

	public async Task<long> Handle(CreateRoomAmenityCommand request, CancellationToken cancellationToken) {
		var entity = new RoomAmenity {
			Name = request.Name
		};

		await context.RoomAmenities.AddAsync(entity, cancellationToken);

		await context.SaveChangesAsync(cancellationToken);

		return entity.Id;
	}
}

