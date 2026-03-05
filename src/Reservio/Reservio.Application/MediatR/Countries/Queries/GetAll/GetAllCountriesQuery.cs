using Reservio.Application.MediatR.Countries.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.Countries.Queries.GetAll;

public class GetAllCountriesQuery : IRequest<IEnumerable<CountryVm>> { }

