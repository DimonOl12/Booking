using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.Languages.Commands.Update;

public class UpdateLanguageCommandHandler(
	IEasyStayDbContext context
) : IRequestHandler<UpdateLanguageCommand> {

	public async Task Handle(UpdateLanguageCommand request, CancellationToken cancellationToken) {
		var entity = await context.Languages.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(Language), request.Id);

		entity.Name = request.Name;
		await context.SaveChangesAsync(cancellationToken);
	}
}
