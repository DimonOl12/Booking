using MediatR;

namespace EasyStay.Application.MediatR.Languages.Commands.Update;

public class UpdateLanguageCommand : IRequest {
	public long Id { get; set; }

	public string Name { get; set; } = null!;
}
