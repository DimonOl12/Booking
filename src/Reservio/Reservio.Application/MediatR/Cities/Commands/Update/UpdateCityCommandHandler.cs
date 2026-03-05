using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.Cities.Commands.Update;

public class UpdateCityCommandHandler(
	IReservioDbContext context,
	IImageService imageService
) : IRequestHandler<UpdateCityCommand> {

	public async Task Handle(UpdateCityCommand request, CancellationToken cancellationToken) {
		var entity = await context.Cities.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(City), request.Id);

		string oldImage = entity.Image;

		entity.Name = request.Name;
		entity.Image = await imageService.SaveImageAsync(request.Image);
		entity.Longitude = request.Longitude;
		entity.Latitude = request.Latitude;
		entity.CountryId = request.CountryId;

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

