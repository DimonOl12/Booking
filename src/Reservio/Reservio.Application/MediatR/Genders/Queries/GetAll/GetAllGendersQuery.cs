using Reservio.Application.MediatR.Genders.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.Genders.Queries.GetAll;

public class GetAllGendersQuery : IRequest<IEnumerable<GenderVm>> { }

