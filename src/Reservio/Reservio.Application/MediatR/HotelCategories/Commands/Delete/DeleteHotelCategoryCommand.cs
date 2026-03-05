using MediatR;

namespace Reservio.Application.MediatR.HotelCategories.Commands.Delete;

public class DeleteHotelCategoryCommand : IRequest {
	public long Id { get; set; }
}

