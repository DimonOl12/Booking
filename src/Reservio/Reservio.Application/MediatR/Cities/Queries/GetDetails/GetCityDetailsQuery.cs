using Reservio.Application.MediatR.Cities.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.Cities.Queries.GetDetails;

public class GetCityDetailsQuery : IRequest<CityVm> {
	public long Id { get; set; }
}

