using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.RentalPeriods.Commands.Update;

public class UpdateRentalPeriodCommandHandler(
	IReservioDbContext context
) : IRequestHandler<UpdateRentalPeriodCommand> {

	public async Task Handle(UpdateRentalPeriodCommand request, CancellationToken cancellationToken) {
		var entity = await context.RentalPeriods.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(RentalPeriod), request.Id);

		entity.Name = request.Name;
		await context.SaveChangesAsync(cancellationToken);
	}
}

