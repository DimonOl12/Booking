using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Chats.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Chats.Queries.GetAll;

public class GetAllChatsQueryHandler(
	IEasyStayDbContext context,
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
