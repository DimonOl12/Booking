using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.HotelAmenities.Commands.Create;

public class CreateHotelAmenityCommandHandler(
	IEasyStayDbContext context,
	IImageService imageService
) : IRequestHandler<CreateHotelAmenityCommand, long> {

	public async Task<long> Handle(CreateHotelAmenityCommand request, CancellationToken cancellationToken) {
		var entity = new HotelAmenity {
			Name = request.Name,
			Image = await imageService.SaveImageAsync(request.Image)
		};

		await context.HotelAmenities.AddAsync(entity, cancellationToken);

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
