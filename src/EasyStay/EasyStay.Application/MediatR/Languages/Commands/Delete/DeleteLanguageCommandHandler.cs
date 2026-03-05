using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.Languages.Commands.Delete;

public class DeleteLanguageCommandHandler(
	IEasyStayDbContext context
) : IRequestHandler<DeleteLanguageCommand> {

	public async Task Handle(DeleteLanguageCommand request, CancellationToken cancellationToken) {
		var entity = await context.Languages.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(Language), request.Id);

		context.Languages.Remove(entity);
		await context.SaveChangesAsync(cancellationToken);
	}
}
