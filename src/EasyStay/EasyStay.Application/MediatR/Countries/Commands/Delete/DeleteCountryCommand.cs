using MediatR;

namespace EasyStay.Application.MediatR.Countries.Commands.Delete;

public class DeleteCountryCommand : IRequest {
	public long Id { get; set; }
}
