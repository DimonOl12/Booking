using Reservio.Application.MediatR.Languages.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.Languages.Queries.GetDetails;

public class GetLanguageDetailsQuery : IRequest<LanguageVm> {
	public long Id { get; set; }
}

