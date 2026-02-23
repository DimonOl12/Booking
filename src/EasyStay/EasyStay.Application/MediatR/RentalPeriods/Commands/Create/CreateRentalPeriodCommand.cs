using MediatR;

namespace EasyStay.Application.MediatR.RentalPeriods.Commands.Create;

public class CreateRentalPeriodCommand : IRequest<long> {
	public string Name { get; set; } = null!;
}
