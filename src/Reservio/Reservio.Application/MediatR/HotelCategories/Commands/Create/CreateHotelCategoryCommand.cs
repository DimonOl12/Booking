using MediatR;

namespace Reservio.Application.MediatR.HotelCategories.Commands.Create;

public class CreateHotelCategoryCommand : IRequest<long> {
	public string Name { get; set; } = null!;
}

