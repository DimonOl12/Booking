using MediatR;

namespace EasyStay.Application.MediatR.Breakfasts.Commands.Delete;

public class DeleteBreakfastCommand : IRequest {
	public long Id { get; set; }
}
