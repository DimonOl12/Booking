using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using MediatR;

namespace Reservio.Application.MediatR.HotelAmenities.Commands.Delete;

public class DeleteHotelAmenityCommandHandler(
	IReservioDbContext context,
	IImageService imageService
) : IRequestHandler<DeleteHotelAmenityCommand> {

	public async Task Handle(DeleteHotelAmenityCommand request, CancellationToken cancellationToken) {
		var entity = await context.HotelAmenities.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(HotelAmenities), request.Id);

		context.HotelAmenities.Remove(entity);
		await context.SaveChangesAsync(cancellationToken);

		imageService.DeleteImageIfExists(entity.Image);
	}
}

