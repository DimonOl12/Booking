using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.Breakfasts.Commands.Delete;

public class DeleteBreakfastCommandHandler(
	IReservioDbContext context
) : IRequestHandler<DeleteBreakfastCommand> {

	public async Task Handle(DeleteBreakfastCommand request, CancellationToken cancellationToken) {
		var entity = await context.Breakfasts.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(Breakfast), request.Id);

		context.Breakfasts.Remove(entity);
		await context.SaveChangesAsync(cancellationToken);
	}
}

