using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.RentalPeriods.Commands.Delete;

public class DeleteRentalPeriodCommandHandler(
	IEasyStayDbContext context
) : IRequestHandler<DeleteRentalPeriodCommand> {

	public async Task Handle(DeleteRentalPeriodCommand request, CancellationToken cancellationToken) {
		var entity = await context.RentalPeriods.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(RentalPeriod), request.Id);

		context.RentalPeriods.Remove(entity);
		await context.SaveChangesAsync(cancellationToken);
	}
}
