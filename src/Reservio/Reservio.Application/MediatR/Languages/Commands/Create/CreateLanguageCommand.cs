using MediatR;

namespace Reservio.Application.MediatR.Languages.Commands.Create;

public class CreateLanguageCommand : IRequest<long> {
	public string Name { get; set; } = null!;
}

