using EasyStay.Application.MediatR.Rooms.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.Rooms.Queries.GetDetails;

public class GetRoomDetailsQuery : IRequest<RoomVm> {
	public long Id { get; set; }
}
