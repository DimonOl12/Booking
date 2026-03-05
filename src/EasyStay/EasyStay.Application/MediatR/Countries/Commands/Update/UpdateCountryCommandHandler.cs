using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.Countries.Commands.Update;

public class UpdateCountryCommandHandler(
	IEasyStayDbContext context,
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
