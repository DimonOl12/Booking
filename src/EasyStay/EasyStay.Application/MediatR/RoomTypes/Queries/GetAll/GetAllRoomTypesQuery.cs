using EasyStay.Application.MediatR.RoomTypes.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.RoomTypes.Queries.GetAll;

public class GetAllRoomTypesQuery : IRequest<IEnumerable<RoomTypeVm>> { }
