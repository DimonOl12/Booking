using MediatR;

namespace EasyStay.Application.MediatR.BankCards.Commands.Delete;

public class DeleteBankCardCommand : IRequest {
	public long Id { get; set; }
}
