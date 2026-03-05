using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.Breakfasts.Commands.Create;

public class CreateBreakfastCommandHandler(
	IReservioDbContext context
) : IRequestHandler<CreateBreakfastCommand, long> {

	public async Task<long> Handle(CreateBreakfastCommand request, CancellationToken cancellationToken) {
		var entity = new Breakfast {
			Name = request.Name,
		};

		await context.Breakfasts.AddAsync(entity, cancellationToken);
		await context.SaveChangesAsync(cancellationToken);

		return entity.Id;
	}
}

