using MediatR;

namespace EasyStay.Application.MediatR.HotelCategories.Commands.Create;

public class CreateHotelCategoryCommand : IRequest<long> {
	public string Name { get; set; } = null!;
}
