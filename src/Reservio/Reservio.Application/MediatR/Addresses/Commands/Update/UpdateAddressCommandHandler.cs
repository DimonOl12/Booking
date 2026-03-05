using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.Addresses.Commands.Update;

public class UpdateAddressCommandHandler(
	IReservioDbContext context
) : IRequestHandler<UpdateAddressCommand> {

	public async Task Handle(UpdateAddressCommand request, CancellationToken cancellationToken) {
		var entity = await context.Addresses
			.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(Address), request.Id);

		entity.Street = request.Street;
		entity.HouseNumber = request.HouseNumber;
		entity.Floor = request.Floor;
		entity.ApartmentNumber = request.ApartmentNumber;
		entity.CityId = request.CityId;

		await context.SaveChangesAsync(cancellationToken);
	}
}
