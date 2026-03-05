using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.RoomAmenities.Commands.Delete;

public class DeleteRoomAmenityCommandHandle(
	IEasyStayDbContext context
) : IRequestHandler<DeleteRoomAmenityCommand> {

	public async Task Handle(DeleteRoomAmenityCommand request, CancellationToken cancellationToken) {
		var enity = await context.RoomAmenities.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(RoomAmenity), request.Id);

		context.RoomAmenities.Remove(enity);
		await context.SaveChangesAsync(cancellationToken);
	}
}
