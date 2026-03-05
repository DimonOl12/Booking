using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.HotelCategories.Commands.Update;

public class UpdateHotelCategoryCommandHandler(
	IReservioDbContext context
) : IRequestHandler<UpdateHotelCategoryCommand> {

	public async Task Handle(UpdateHotelCategoryCommand request, CancellationToken cancellationToken) {
		var entity = await context.HotelCategories.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(HotelCategory), request.Id);

		entity.Name = request.Name;
		await context.SaveChangesAsync(cancellationToken);
	}
}

