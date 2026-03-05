using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.Countries.Commands.Delete;

public class DeleteCountryCommandHandler(
	IEasyStayDbContext context,
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
