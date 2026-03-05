using EasyStay.Application.MediatR.RoomTypes.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.RoomTypes.Queries.GetDetails;

public class GetRoomTypeDetailsQuery : IRequest<RoomTypeVm> {
	public long Id { get; set; }
}
