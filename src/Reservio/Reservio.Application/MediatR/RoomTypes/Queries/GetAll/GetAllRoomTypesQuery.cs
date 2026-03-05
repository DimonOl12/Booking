using Reservio.Application.MediatR.RoomTypes.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.RoomTypes.Queries.GetAll;

public class GetAllRoomTypesQuery : IRequest<IEnumerable<RoomTypeVm>> { }

