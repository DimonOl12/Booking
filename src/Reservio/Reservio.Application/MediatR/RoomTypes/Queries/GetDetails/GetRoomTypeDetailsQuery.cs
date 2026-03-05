using Reservio.Application.MediatR.RoomTypes.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.RoomTypes.Queries.GetDetails;

public class GetRoomTypeDetailsQuery : IRequest<RoomTypeVm> {
	public long Id { get; set; }
}

