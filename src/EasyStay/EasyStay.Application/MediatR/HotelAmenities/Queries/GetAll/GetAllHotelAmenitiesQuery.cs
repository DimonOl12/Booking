using EasyStay.Application.MediatR.HotelAmenities.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.HotelAmenities.Queries.GetAll;

public class GetAllHotelAmenitiesQuery : IRequest<IEnumerable<HotelAmenityVm>> { }
