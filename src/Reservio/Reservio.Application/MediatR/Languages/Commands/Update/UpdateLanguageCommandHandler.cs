using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.Languages.Commands.Update;

public class UpdateLanguageCommandHandler(
	IReservioDbContext context
) : IRequestHandler<UpdateLanguageCommand> {

	public async Task Handle(UpdateLanguageCommand request, CancellationToken cancellationToken) {
		var entity = await context.Languages.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(Language), request.Id);

		entity.Name = request.Name;
		await context.SaveChangesAsync(cancellationToken);
	}
}

