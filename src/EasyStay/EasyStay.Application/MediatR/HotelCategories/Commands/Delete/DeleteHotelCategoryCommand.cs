using MediatR;

namespace EasyStay.Application.MediatR.HotelCategories.Commands.Delete;

public class DeleteHotelCategoryCommand : IRequest {
	public long Id { get; set; }
}
