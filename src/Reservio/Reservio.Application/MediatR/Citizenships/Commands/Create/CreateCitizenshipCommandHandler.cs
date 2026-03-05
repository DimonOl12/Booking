using Reservio.Application.Interfaces;
using Reservio.Domain.Entities;
using MediatR;

namespace Reservio.Application.MediatR.Citizenships.Commands.Create;

public class CreateCitizenshipCommandHandler(
	IReservioDbContext context
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

