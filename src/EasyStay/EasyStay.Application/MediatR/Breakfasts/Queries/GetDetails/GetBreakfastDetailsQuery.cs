using EasyStay.Application.MediatR.Breakfasts.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.Breakfasts.Queries.GetDetails;

public class GetBreakfastDetailsQuery : IRequest<BreakfastVm> {
	public long Id { get; set; }
}
