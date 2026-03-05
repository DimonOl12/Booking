using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.Cities.Commands.Delete;

public class DeleteCityCommandHandler(
	IReservioDbContext context,
	IImageService imageService
) : IRequestHandler<DeleteCityCommand> {

	public async Task Handle(DeleteCityCommand request, CancellationToken cancellationToken) {
		var entity = await context.Cities.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(City), request.Id);

		context.Cities.Remove(entity);
		await context.SaveChangesAsync(cancellationToken);

		imageService.DeleteImageIfExists(entity.Image);
	}
}

