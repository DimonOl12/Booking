using EasyStay.Application.MediatR.Cities.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.Cities.Queries.GetDetails;

public class GetCityDetailsQuery : IRequest<CityVm> {
	public long Id { get; set; }
}
