using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.BankCards.Commands.Delete;

public class DeleteBankCardCommandHandler(
	IReservioDbContext context,
	ICurrentUserService currentUserService
) : IRequestHandler<DeleteBankCardCommand> {

	public async Task Handle(DeleteBankCardCommand request, CancellationToken cancellationToken) {
		var countOfDeleted = await context.BankCards
			.Where(bc => bc.Id == request.Id && bc.CustomerId == currentUserService.GetRequiredUserId())
			.ExecuteDeleteAsync(cancellationToken);

		if (countOfDeleted == 0)
			throw new NotFoundException(nameof(BankCard), request.Id);
	}
}

