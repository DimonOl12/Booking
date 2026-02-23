using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.BankCards.Commands.Delete;

public class DeleteBankCardCommandHandler(
	IEasyStayDbContext context,
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
