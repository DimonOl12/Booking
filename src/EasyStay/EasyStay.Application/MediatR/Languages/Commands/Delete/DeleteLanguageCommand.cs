using MediatR;

namespace EasyStay.Application.MediatR.Languages.Commands.Delete;

public class DeleteLanguageCommand : IRequest {
	public long Id { get; set; }
}
