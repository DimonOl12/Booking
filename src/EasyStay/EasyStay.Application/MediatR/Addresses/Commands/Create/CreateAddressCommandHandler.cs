using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.Addresses.Commands.Create;

public class CreateAddressCommandHandler(
	IEasyStayDbContext context
) : IRequestHandler<CreateAddressCommand, long> {

	public async Task<long> Handle(CreateAddressCommand request, CancellationToken cancellationToken) {
		var entity = new Address {
			Street = request.Street,
			HouseNumber = request.HouseNumber,
			Floor = request.Floor,
			ApartmentNumber = request.ApartmentNumber,
			CityId = request.CityId
		};

		await context.Addresses.AddAsync(entity, cancellationToken);
		await context.SaveChangesAsync(cancellationToken);

		return entity.Id;
	}
}