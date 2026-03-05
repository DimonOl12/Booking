using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.Citizenships.Commands.Delete;

public class DeleteCitizenshipCommandHandle(
	IReservioDbContext context
) : IRequestHandler<DeleteCitizenshipCommand> {

	public async Task Handle(DeleteCitizenshipCommand request, CancellationToken cancellationToken) {
		var enity = await context.Citizenships.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(Citizenship), request.Id);

		context.Citizenships.Remove(enity);
		await context.SaveChangesAsync(cancellationToken);
	}
}

