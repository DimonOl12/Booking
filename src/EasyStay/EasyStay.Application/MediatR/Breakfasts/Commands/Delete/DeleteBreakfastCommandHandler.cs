using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.Breakfasts.Commands.Delete;

public class DeleteBreakfastCommandHandler(
	IEasyStayDbContext context
) : IRequestHandler<DeleteBreakfastCommand> {

	public async Task Handle(DeleteBreakfastCommand request, CancellationToken cancellationToken) {
		var entity = await context.Breakfasts.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(Breakfast), request.Id);

		context.Breakfasts.Remove(entity);
		await context.SaveChangesAsync(cancellationToken);
	}
}
