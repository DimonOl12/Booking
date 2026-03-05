using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.Countries.Commands.Delete;

public class DeleteCountryCommandHandler(
	IReservioDbContext context,
	IImageService imageService
) : IRequestHandler<DeleteCountryCommand> {

	public async Task Handle(DeleteCountryCommand request, CancellationToken cancellationToken) {
		var entity = await context.Countries.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(Country), request.Id);

		context.Countries.Remove(entity);
		await context.SaveChangesAsync(cancellationToken);

		imageService.DeleteImageIfExists(entity.Image);
	}
}

