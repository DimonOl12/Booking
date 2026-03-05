using Reservio.Application.MediatR.Breakfasts.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.Breakfasts.Queries.GetDetails;

public class GetBreakfastDetailsQuery : IRequest<BreakfastVm> {
	public long Id { get; set; }
}

