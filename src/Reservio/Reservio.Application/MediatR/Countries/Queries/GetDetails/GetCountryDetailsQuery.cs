using Reservio.Application.MediatR.Countries.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.Countries.Queries.GetDetails;

public class GetCountryDetailsQuery : IRequest<CountryVm> {
	public long Id { get; set; }
}

