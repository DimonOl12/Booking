using MediatR;

namespace EasyStay.Application.MediatR.Citizenships.Commands.Delete;

public class DeleteCitizenshipCommand : IRequest {
	public long Id { get; set; }
}
