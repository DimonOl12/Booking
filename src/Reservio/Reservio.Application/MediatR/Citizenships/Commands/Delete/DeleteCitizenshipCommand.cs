using MediatR;

namespace Reservio.Application.MediatR.Citizenships.Commands.Delete;

public class DeleteCitizenshipCommand : IRequest {
	public long Id { get; set; }
}

