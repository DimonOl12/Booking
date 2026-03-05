using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.BankCards.Commands.Create;

public class CreateBankCardCommandHandler(
	IReservioDbContext context,
	IDateConverter dateConverter,
	ICurrentUserService currentUserService
) : IRequestHandler<CreateBankCardCommand, long> {

	public async Task<long> Handle(CreateBankCardCommand request, CancellationToken cancellationToken) {
		var entity = new BankCard {
			Number = request.Number,
			ExpirationDate = dateConverter.ToFirstDayOfMonth(request.ExpirationDate),
			Cvv = request.Cvv,
			OwnerFullName = request.OwnerFullName,
			CustomerId = currentUserService.GetRequiredUserId()
		};

		await context.BankCards.AddAsync(entity, cancellationToken);
		await context.SaveChangesAsync(cancellationToken);

		return entity.Id;
	}
}

