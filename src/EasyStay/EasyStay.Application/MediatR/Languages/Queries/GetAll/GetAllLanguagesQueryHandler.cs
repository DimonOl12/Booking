using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Languages.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Languages.Queries.GetAll;

public class GetAllLanguagesQueryHandler(
	IEasyStayDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllLanguagesQuery, IEnumerable<LanguageVm>> {

	public async Task<IEnumerable<LanguageVm>> Handle(GetAllLanguagesQuery request, CancellationToken cancellationToken) {
		var items = await context.Languages
			.AsNoTracking()
			.ProjectTo<LanguageVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}
