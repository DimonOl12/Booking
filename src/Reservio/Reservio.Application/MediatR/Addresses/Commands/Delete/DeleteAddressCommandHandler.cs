using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.Addresses.Commands.Delete;

public class DeleteAddressCommandHandler(
	IReservioDbContext context
) : IRequestHandler<DeleteAddressCommand> {

	public async Task Handle(DeleteAddressCommand request, CancellationToken cancellationToken) {
		var entity = await context.Addresses
			.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(Address), request.Id);

		context.Addresses.Remove(entity);
		await context.SaveChangesAsync(cancellationToken);
	}
}
