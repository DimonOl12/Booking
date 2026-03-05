using Reservio.Application.MediatR.Cities.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.Cities.Queries.GetAll;

public class GetAllCitiesQuery : IRequest<IEnumerable<CityVm>> { }

