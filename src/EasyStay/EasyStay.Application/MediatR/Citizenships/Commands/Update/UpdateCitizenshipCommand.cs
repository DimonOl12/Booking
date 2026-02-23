using MediatR;

namespace EasyStay.Application.MediatR.Citizenships.Commands.Update;

public class UpdateCitizenshipCommand : IRequest {
	public long Id { get; set; }

	public string Name { get; set; } = null!;
}
