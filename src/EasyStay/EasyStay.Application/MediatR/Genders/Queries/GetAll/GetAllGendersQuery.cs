using EasyStay.Application.MediatR.Genders.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.Genders.Queries.GetAll;

public class GetAllGendersQuery : IRequest<IEnumerable<GenderVm>> { }
