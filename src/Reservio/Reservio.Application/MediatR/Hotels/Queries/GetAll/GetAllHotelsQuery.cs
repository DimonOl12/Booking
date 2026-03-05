using Reservio.Application.MediatR.Hotels.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.Hotels.Queries.GetAll;

public class GetAllHotelsQuery : IRequest<IEnumerable<HotelVm>> { }

