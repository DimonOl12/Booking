using Reservio.Application.MediatR.Languages.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.Languages.Queries.GetAll;

public class GetAllLanguagesQuery : IRequest<IEnumerable<LanguageVm>> { }

