using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.Cities.Commands.Delete;

public class DeleteCityCommandHandler(
	IEasyStayDbContext context,
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
