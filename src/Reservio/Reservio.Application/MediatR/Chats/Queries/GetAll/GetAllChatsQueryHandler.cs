using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Chats.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.Chats.Queries.GetAll;

public class GetAllChatsQueryHandler(
	IReservioDbContext context,
	IMapper mapper,
	ICurrentUserService currentUserService
) : IRequestHandler<GetAllChatsQuery, IEnumerable<ChatVm>> {

	public async Task<IEnumerable<ChatVm>> Handle(GetAllChatsQuery request, CancellationToken cancellationToken) {
		var userId = currentUserService.GetRequiredUserId();

		var items = await context.Chats
			.AsNoTracking()
			.Where(c => c.RealtorId == userId || c.CustomerId == userId)
			.ProjectTo<ChatVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}

