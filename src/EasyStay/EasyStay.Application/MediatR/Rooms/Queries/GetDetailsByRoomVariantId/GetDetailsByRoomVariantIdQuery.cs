using EasyStay.Application.MediatR.Rooms.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.Rooms.Queries.GetDetailsByRoomVariantId;

public class GetDetailsByRoomVariantIdQuery : IRequest<RoomVm> {
	public long RoomVariantId { get; set; }
}
