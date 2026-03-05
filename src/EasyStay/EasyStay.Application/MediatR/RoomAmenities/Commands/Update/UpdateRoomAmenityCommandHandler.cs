using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.RoomAmenities.Commands.Update;

public class UpdateRoomAmenityCommandHandler(
	IEasyStayDbContext context
) : IRequestHandler<UpdateRoomAmenityCommand> {

	public async Task Handle(UpdateRoomAmenityCommand request, CancellationToken cancellationToken) {
		var entity = await context.RoomAmenities.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(RoomAmenity), request.Id);

		entity.Name = request.Name;

		await context.SaveChangesAsync(cancellationToken);
	}
}
