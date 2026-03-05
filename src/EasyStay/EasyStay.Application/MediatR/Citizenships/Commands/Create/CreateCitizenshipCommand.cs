using MediatR;

namespace EasyStay.Application.MediatR.Citizenships.Commands.Create;

public class CreateCitizenshipCommand : IRequest<long> {
	public string Name { get; set; } = null!;
}
