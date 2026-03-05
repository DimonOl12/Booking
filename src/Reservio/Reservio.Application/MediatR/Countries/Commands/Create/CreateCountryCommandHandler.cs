using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.Countries.Commands.Create;

public class CreateCountryCommandHandler(
	IReservioDbContext context,
	IImageService imageService
) : IRequestHandler<CreateCountryCommand, long> {

	public async Task<long> Handle(CreateCountryCommand request, CancellationToken cancellationToken) {
		var entity = new Country {
			Name = request.Name,
			Image = await imageService.SaveImageAsync(request.Image)
		};

		await context.Countries.AddAsync(entity, cancellationToken);

		try {
			await context.SaveChangesAsync(cancellationToken);
		}
		catch {
			imageService.DeleteImageIfExists(entity.Image);
			throw;
		}

		return entity.Id;
	}
}

