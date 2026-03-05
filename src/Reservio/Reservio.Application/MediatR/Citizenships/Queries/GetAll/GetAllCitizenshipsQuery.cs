using Reservio.Application.MediatR.Citizenships.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.Citizenships.Queries.GetAll;

public class GetAllCitizenshipsQuery : IRequest<IEnumerable<CitizenshipVm>> { }

