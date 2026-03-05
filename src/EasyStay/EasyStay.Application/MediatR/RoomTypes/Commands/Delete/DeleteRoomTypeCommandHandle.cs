using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.RoomTypes.Commands.Delete;

public class DeleteRoomTypeCommandHandle(
	IEasyStayDbContext context
) : IRequestHandler<DeleteRoomTypeCommand> {

	public async Task Handle(DeleteRoomTypeCommand request, CancellationToken cancellationToken) {
		var enity = await context.RoomTypes.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(RoomType), request.Id);

		context.RoomTypes.Remove(enity);
		await context.SaveChangesAsync(cancellationToken);
	}
}
