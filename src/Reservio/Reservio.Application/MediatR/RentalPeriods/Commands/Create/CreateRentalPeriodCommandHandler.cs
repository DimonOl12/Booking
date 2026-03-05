using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.RentalPeriods.Commands.Create;

public class CreateRentalPeriodCommandHandler(
	IReservioDbContext context
) : IRequestHandler<CreateRentalPeriodCommand, long> {

	public async Task<long> Handle(CreateRentalPeriodCommand request, CancellationToken cancellationToken) {
		var entity = new RentalPeriod {
			Name = request.Name,
		};

		await context.RentalPeriods.AddAsync(entity, cancellationToken);
		await context.SaveChangesAsync(cancellationToken);

		return entity.Id;
	}
}

