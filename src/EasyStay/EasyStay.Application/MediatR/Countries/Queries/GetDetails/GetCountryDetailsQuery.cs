using EasyStay.Application.MediatR.Countries.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.Countries.Queries.GetDetails;

public class GetCountryDetailsQuery : IRequest<CountryVm> {
	public long Id { get; set; }
}
