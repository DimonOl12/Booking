using MediatR;

namespace EasyStay.Application.MediatR.Breakfasts.Commands.Create;

public class CreateBreakfastCommand : IRequest<long> {
	public string Name { get; set; } = null!;
}
