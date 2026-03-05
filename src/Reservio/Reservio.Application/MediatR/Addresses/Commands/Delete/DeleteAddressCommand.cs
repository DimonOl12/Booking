using MediatR;

namespace Reservio.Application.MediatR.Addresses.Commands.Delete;

public class DeleteAddressCommand : IRequest {
	public long Id { get; set; }
}

