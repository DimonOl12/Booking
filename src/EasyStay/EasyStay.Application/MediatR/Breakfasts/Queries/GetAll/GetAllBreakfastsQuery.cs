using EasyStay.Application.MediatR.Breakfasts.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.Breakfasts.Queries.GetAll;

public class GetAllBreakfastsQuery : IRequest<IEnumerable<BreakfastVm>> { }
