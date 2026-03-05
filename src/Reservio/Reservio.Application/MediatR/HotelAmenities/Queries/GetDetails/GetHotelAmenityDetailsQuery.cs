using Reservio.Application.MediatR.HotelAmenities.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.HotelAmenities.Queries.GetDetails;

public class GetHotelAmenityDetailsQuery : IRequest<HotelAmenityVm> {
	public long Id { get; set; }
}

