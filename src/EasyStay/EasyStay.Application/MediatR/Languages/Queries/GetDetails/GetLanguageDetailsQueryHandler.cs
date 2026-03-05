using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Languages.Queries.Shared;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Languages.Queries.GetDetails;

public class GetLanguageDetailsQueryHandler(
	IEasyStayDbContext context,
	IMapper mapper
) : IRequestHandler<GetLanguageDetailsQuery, LanguageVm> {

	public async Task<LanguageVm> Handle(GetLanguageDetailsQuery request, CancellationToken cancellationToken) {
		var vm = await context.Languages
			.AsNoTracking()
			.ProjectTo<LanguageVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(Language), request.Id);

		return vm;
	}
}
