using EasyStay.Application.MediatR.Languages.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.Languages.Queries.GetDetails;

public class GetLanguageDetailsQuery : IRequest<LanguageVm> {
	public long Id { get; set; }
}
