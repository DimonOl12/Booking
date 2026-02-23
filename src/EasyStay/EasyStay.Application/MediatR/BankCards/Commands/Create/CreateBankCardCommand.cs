using MediatR;

namespace EasyStay.Application.MediatR.BankCards.Commands.Create;

public class CreateBankCardCommand : IRequest<long> {
	public string Number { get; set; } = null!;

	public DateOnly ExpirationDate { get; set; }

	public string Cvv { get; set; } = null!;

	public string OwnerFullName { get; set; } = null!;
}
