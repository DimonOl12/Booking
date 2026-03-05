using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.Languages.Commands.Delete;

public class DeleteLanguageCommandHandler(
	IReservioDbContext context
) : IRequestHandler<DeleteLanguageCommand> {

	public async Task Handle(DeleteLanguageCommand request, CancellationToken cancellationToken) {
		var entity = await context.Languages.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(Language), request.Id);

		context.Languages.Remove(entity);
		await context.SaveChangesAsync(cancellationToken);
	}
}

