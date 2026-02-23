using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.Citizenships.Commands.Update;

public class UpdateCitizenshipCommandHandler(
	IEasyStayDbContext context
) : IRequestHandler<UpdateCitizenshipCommand> {

	public async Task Handle(UpdateCitizenshipCommand request, CancellationToken cancellationToken) {
		var entity = await context.Citizenships.FindAsync([request.Id], cancellationToken)
			?? throw new NotFoundException(nameof(Citizenship), request.Id);

		entity.Name = request.Name;

		await context.SaveChangesAsync(cancellationToken);
	}
}
