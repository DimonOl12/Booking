using MediatR;

namespace EasyStay.Application.MediatR.Hotels.Commands.Delete;

public class DeleteHotelCommand : IRequest {
	public long Id { get; set; }
}
