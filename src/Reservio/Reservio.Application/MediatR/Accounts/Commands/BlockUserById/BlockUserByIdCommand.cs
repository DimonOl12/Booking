using MediatR;

namespace Reservio.Application.MediatR.Accounts.Commands.BlockUserById;

public class BlockUserByIdCommand : IRequest {
	public long Id { get; set; }

	public DateTime LockoutEndUtc { get; set; }
}

