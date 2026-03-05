using Reservio.Application.MediatR.HotelCategories.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.HotelCategories.Queries.GetDetails;

public class GetHotelCategoryDetailsQuery : IRequest<HotelCategoryVm> {
	public long Id { get; set; }
}

