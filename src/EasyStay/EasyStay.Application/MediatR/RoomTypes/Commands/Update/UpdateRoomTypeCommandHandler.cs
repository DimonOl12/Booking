using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.RoomTypes.Commands.Update;

public class UpdateRoomTypeCommandHandler(
	IEasyStayDbContext context
) : IRequestHandler<UpdateRoomTypeCommand> {

	public async Task Handle(UpdateRoomTypeCommand request, CancellationToken cancellationToken) {
		var entity = await context.RoomTypes.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(RoomType), request.Id);

		entity.Name = request.Name;

		await context.SaveChangesAsync(cancellationToken);
	}
}
