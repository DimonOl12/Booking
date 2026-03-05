using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.Citizenships.Commands.Update;

public class UpdateCitizenshipCommandHandler(
	IReservioDbContext context
) : IRequestHandler<UpdateCitizenshipCommand> {

	public async Task Handle(UpdateCitizenshipCommand request, CancellationToken cancellationToken) {
		var entity = await context.Citizenships.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(Citizenship), request.Id);

		entity.Name = request.Name;

		await context.SaveChangesAsync(cancellationToken);
	}
}

