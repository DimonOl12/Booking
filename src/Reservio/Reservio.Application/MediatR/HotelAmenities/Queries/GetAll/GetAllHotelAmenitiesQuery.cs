using Reservio.Application.MediatR.HotelAmenities.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.HotelAmenities.Queries.GetAll;

public class GetAllHotelAmenitiesQuery : IRequest<IEnumerable<HotelAmenityVm>> { }

