using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Genders.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.Genders.Queries.GetAll;

public class GetAllGendersQueryHandler(
	IReservioDbContext context,
	IMapper mapper
) : IRequestHandler<GetAllGendersQuery, IEnumerable<GenderVm>> {

	public async Task<IEnumerable<GenderVm>> Handle(GetAllGendersQuery request, CancellationToken cancellationToken) {
		var items = await context.Genders
			.AsNoTracking()
			.ProjectTo<GenderVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}

