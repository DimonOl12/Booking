using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.Citizenships.Commands.Create;

public class CreateCitizenshipCommandHandler(
	IEasyStayDbContext context
) : IRequestHandler<CreateCitizenshipCommand, long> {

	public async Task<long> Handle(CreateCitizenshipCommand request, CancellationToken cancellationToken) {
		var entity = new Citizenship {
			Name = request.Name
		};

		await context.Citizenships.AddAsync(entity, cancellationToken);

		await context.SaveChangesAsync(cancellationToken);

		return entity.Id;
	}
}
