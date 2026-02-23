using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using MediatR;

namespace EasyStay.Application.MediatR.HotelAmenities.Commands.Delete;

public class DeleteHotelAmenityCommandHandler(
	IEasyStayDbContext context,
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
