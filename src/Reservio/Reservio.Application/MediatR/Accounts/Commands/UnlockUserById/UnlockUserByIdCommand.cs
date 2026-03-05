using MediatR;

namespace Reservio.Application.MediatR.Accounts.Commands.UnlockUserById;

public class UnlockUserByIdCommand : IRequest {
	public long Id { get; set; }
}

