using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.HotelCategories.Commands.Delete;

public class DeleteHotelCategoryCommandHandler(
	IReservioDbContext context
) : IRequestHandler<DeleteHotelCategoryCommand> {

	public async Task Handle(DeleteHotelCategoryCommand request, CancellationToken cancellationToken) {
		var entity = await context.HotelCategories.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(HotelCategory), request.Id);

		context.HotelCategories.Remove(entity);
		await context.SaveChangesAsync(cancellationToken);
	}
}

