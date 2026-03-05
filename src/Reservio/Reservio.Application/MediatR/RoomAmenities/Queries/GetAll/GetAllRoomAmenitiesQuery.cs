using Reservio.Application.MediatR.RoomAmenities.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.RoomAmenities.Queries.GetAll;

public class GetAllRoomAmenitiesQuery : IRequest<IEnumerable<RoomAmenityVm>> { }

