using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.Breakfasts.Commands.Update;

public class UpdateBreakfastCommandHandler(
	IReservioDbContext context
) : IRequestHandler<UpdateBreakfastCommand> {

	public async Task Handle(UpdateBreakfastCommand request, CancellationToken cancellationToken) {
		var entity = await context.Breakfasts.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(Breakfast), request.Id);

		entity.Name = request.Name;
		await context.SaveChangesAsync(cancellationToken);
	}
}

