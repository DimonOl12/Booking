using EasyStay.Application.MediatR.Hotels.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.Hotels.Queries.GetAll;

public class GetAllHotelsQuery : IRequest<IEnumerable<HotelVm>> { }
