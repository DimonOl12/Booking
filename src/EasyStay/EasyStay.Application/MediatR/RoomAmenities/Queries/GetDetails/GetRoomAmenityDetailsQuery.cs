using EasyStay.Application.MediatR.RoomAmenities.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.RoomAmenities.Queries.GetDetails;

public class GetRoomAmenityDetailsQuery : IRequest<RoomAmenityVm> {
	public long Id { get; set; }
}
