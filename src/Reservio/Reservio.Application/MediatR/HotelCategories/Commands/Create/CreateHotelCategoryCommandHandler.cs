using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.HotelCategories.Commands.Create;

public class CreateHotelCategoryCommandHandler(
	IReservioDbContext context
) : IRequestHandler<CreateHotelCategoryCommand, long> {

	public async Task<long> Handle(CreateHotelCategoryCommand request, CancellationToken cancellationToken) {
		var entity = new HotelCategory {
			Name = request.Name
		};

		await context.HotelCategories.AddAsync(entity, cancellationToken);
		await context.SaveChangesAsync(cancellationToken);

		return entity.Id;
	}
}

