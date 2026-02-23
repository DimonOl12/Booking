using MediatR;

namespace EasyStay.Application.MediatR.Accounts.Commands.BlockUserById;

public class BlockUserByIdCommand : IRequest {
	public long Id { get; set; }

	public DateTime LockoutEndUtc { get; set; }
}
