using MediatR;

namespace Reservio.Application.MediatR.Citizenships.Commands.Create;

public class CreateCitizenshipCommand : IRequest<long> {
	public string Name { get; set; } = null!;
}

