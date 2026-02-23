using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.Countries.Commands.Create;

public class CreateCountryCommandHandler(
	IEasyStayDbContext context,
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
