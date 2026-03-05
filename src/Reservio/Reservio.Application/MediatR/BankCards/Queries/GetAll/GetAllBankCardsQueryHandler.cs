using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.BankCards.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.BankCards.Queries.GetAll;

public class GetAllBankCardsQueryHandler(
	IReservioDbContext context,
	IMapper mapper,
	ICurrentUserService currentUser
) : IRequestHandler<GetAllBankCardsQuery, IEnumerable<BankCardVm>> {

	public async Task<IEnumerable<BankCardVm>> Handle(GetAllBankCardsQuery request, CancellationToken cancellationToken) {
		var items = await context.BankCards
			.AsNoTracking()
			.Where(bc => bc.CustomerId == currentUser.GetRequiredUserId())
			.ProjectTo<BankCardVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}

