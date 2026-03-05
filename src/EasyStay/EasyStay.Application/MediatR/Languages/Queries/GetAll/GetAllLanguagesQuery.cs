using EasyStay.Application.MediatR.Languages.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.Languages.Queries.GetAll;

public class GetAllLanguagesQuery : IRequest<IEnumerable<LanguageVm>> { }
