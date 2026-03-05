using MediatR;

namespace EasyStay.Application.MediatR.RentalPeriods.Commands.Update;

public class UpdateRentalPeriodCommand : IRequest {
	public long Id { get; set; }

	public string Name { get; set; } = null!;
}
