using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.RentalPeriods.Commands.Create;

public class CreateRentalPeriodCommandHandler(
	IEasyStayDbContext context
) : IRequestHandler<CreateRentalPeriodCommand, long> {

	public async Task<long> Handle(CreateRentalPeriodCommand request, CancellationToken cancellationToken) {
		var entity = new RentalPeriod {
			Name = request.Name,
		};

		await context.RentalPeriods.AddAsync(entity, cancellationToken);
		await context.SaveChangesAsync(cancellationToken);

		return entity.Id;
	}
}
