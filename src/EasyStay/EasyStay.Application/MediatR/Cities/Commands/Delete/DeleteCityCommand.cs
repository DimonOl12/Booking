using MediatR;

namespace EasyStay.Application.MediatR.Cities.Commands.Delete;

public class DeleteCityCommand : IRequest {
	public long Id { get; set; }
}
