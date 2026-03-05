using Reservio.Application.MediatR.Breakfasts.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.Breakfasts.Queries.GetAll;

public class GetAllBreakfastsQuery : IRequest<IEnumerable<BreakfastVm>> { }

