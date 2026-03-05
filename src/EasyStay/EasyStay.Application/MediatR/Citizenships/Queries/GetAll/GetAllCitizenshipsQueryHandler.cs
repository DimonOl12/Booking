using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Citizenships.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Citizenships.Queries.GetAll;

public class GetAllCitizenshipsQueryHandler(
	IEasyStayDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllCitizenshipsQuery, IEnumerable<CitizenshipVm>> {

	public async Task<IEnumerable<CitizenshipVm>> Handle(GetAllCitizenshipsQuery request, CancellationToken cancellationToken) {
		var items = await context.Citizenships
			.AsNoTracking()
			.ProjectTo<CitizenshipVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}
