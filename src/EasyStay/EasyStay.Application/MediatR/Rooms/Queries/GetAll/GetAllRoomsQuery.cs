using EasyStay.Application.MediatR.Rooms.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.Rooms.Queries.GetAll;

public class GetAllRoomsQuery : IRequest<IEnumerable<RoomVm>> { }
