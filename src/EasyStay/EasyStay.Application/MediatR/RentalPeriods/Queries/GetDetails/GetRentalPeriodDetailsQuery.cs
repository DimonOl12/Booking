using EasyStay.Application.MediatR.RentalPeriods.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.RentalPeriods.Queries.GetDetails;

public class GetRentalPeriodDetailsQuery : IRequest<RentalPeriodVm> {
	public long Id { get; set; }
}
