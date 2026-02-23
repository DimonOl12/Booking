using MediatR;

namespace EasyStay.Application.MediatR.Addresses.Commands.Create;

public class CreateAddressCommand : IRequest<long> {
	public string Street { get; set; } = null!;

	public string HouseNumber { get; set; } = null!;

	public int? Floor { get; set; }

	public string? ApartmentNumber { get; set; }

	public long CityId { get; set; }
}