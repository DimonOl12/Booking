using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.RentalPeriods.Commands.Delete;

public class DeleteRentalPeriodCommandHandler(
	IReservioDbContext context
) : IRequestHandler<DeleteRentalPeriodCommand> {

	public async Task Handle(DeleteRentalPeriodCommand request, CancellationToken cancellationToken) {
		var entity = await context.RentalPeriods.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(RentalPeriod), request.Id);

		context.RentalPeriods.Remove(entity);
		await context.SaveChangesAsync(cancellationToken);
	}
}

