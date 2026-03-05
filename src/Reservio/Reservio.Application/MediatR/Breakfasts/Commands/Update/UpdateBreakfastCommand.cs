using MediatR;

namespace Reservio.Application.MediatR.Breakfasts.Commands.Update;

public class UpdateBreakfastCommand : IRequest {
	public long Id { get; set; }

	public string Name { get; set; } = null!;
}

