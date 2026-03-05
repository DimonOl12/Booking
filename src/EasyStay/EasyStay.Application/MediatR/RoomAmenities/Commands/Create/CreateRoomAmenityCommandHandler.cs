using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.RoomAmenities.Commands.Create;

public class CreateRoomAmenityCommandHandler(
	IEasyStayDbContext context
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
