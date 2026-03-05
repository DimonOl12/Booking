using EasyStay.Application.MediatR.RoomAmenities.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.RoomAmenities.Queries.GetAll;

public class GetAllRoomAmenitiesQuery : IRequest<IEnumerable<RoomAmenityVm>> { }
