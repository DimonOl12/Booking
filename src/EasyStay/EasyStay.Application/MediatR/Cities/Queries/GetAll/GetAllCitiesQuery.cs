using EasyStay.Application.MediatR.Cities.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.Cities.Queries.GetAll;

public class GetAllCitiesQuery : IRequest<IEnumerable<CityVm>> { }
