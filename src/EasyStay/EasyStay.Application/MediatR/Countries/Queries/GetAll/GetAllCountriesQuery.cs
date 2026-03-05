using EasyStay.Application.MediatR.Countries.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.Countries.Queries.GetAll;

public class GetAllCountriesQuery : IRequest<IEnumerable<CountryVm>> { }
