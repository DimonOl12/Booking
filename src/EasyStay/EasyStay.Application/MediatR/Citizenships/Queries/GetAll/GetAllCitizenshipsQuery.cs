using EasyStay.Application.MediatR.Citizenships.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.Citizenships.Queries.GetAll;

public class GetAllCitizenshipsQuery : IRequest<IEnumerable<CitizenshipVm>> { }
