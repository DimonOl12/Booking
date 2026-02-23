using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.Citizenships.Commands.Delete;

public class DeleteCitizenshipCommandHandle(
	IEasyStayDbContext context
) : IRequestHandler<DeleteCitizenshipCommand> {

	public async Task Handle(DeleteCitizenshipCommand request, CancellationToken cancellationToken) {
		var enity = await context.Citizenships.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(Citizenship), request.Id);

		context.Citizenships.Remove(enity);
		await context.SaveChangesAsync(cancellationToken);
	}
}
