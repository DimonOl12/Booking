using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Addresses.Commands.Delete;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Hotels.Commands.Delete;

public class DeleteHotelCommandHandler(
	IEasyStayDbContext context,
	IImageService imageService,
	IMediator mediator,
	ICurrentUserService currentUserService
) : IRequestHandler<DeleteHotelCommand> {

	public async Task Handle(DeleteHotelCommand request, CancellationToken cancellationToken) {
		var entity = await context.Hotels
			.Include(h => h.Photos)
			.FirstOrDefaultAsync(
				h => h.Id == request.Id && h.RealtorId == currentUserService.GetRequiredUserId(),
				cancellationToken
			)
			?? throw new NotFoundException(nameof(Hotels), request.Id);

		context.Hotels.Remove(entity);
		await context.SaveChangesAsync(cancellationToken);

		imageService.DeleteImagesIfExists(entity.Photos.Select(p => p.Name));

		await mediator.Send(new DeleteAddressCommand { Id = entity.AddressId }, cancellationToken);
	}
}
