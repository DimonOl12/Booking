using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.Countries.Commands.Update;

public class UpdateCountryCommandHandler(
	IReservioDbContext context,
	IImageService imageService
) : IRequestHandler<UpdateCountryCommand> {

	public async Task Handle(UpdateCountryCommand request, CancellationToken cancellationToken) {
		var entity = await context.Countries.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(Country), request.Id);

		string oldImage = entity.Image;

		entity.Name = request.Name;
		entity.Image = await imageService.SaveImageAsync(request.Image);

		try {
			await context.SaveChangesAsync(cancellationToken);

			imageService.DeleteImageIfExists(oldImage);
		}
		catch {
			imageService.DeleteImageIfExists(entity.Image);
			throw;
		}
	}
}

