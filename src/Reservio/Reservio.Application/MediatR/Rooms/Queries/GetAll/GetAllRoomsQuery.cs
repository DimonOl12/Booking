using Reservio.Application.MediatR.Rooms.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.Rooms.Queries.GetAll;

public class GetAllRoomsQuery : IRequest<IEnumerable<RoomVm>> { }

