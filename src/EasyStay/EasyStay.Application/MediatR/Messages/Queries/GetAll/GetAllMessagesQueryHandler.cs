using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Messages.Queries.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Messages.Queries.GetAll;

public class GetAllMessagesQueryHandler(
	IEasyStayDbContext context,
	IMapper mapper,
	ICurrentUserService currentUserService
) : IRequestHandler<GetAllMessagesQuery, IEnumerable<MessageVm>> {

	public async Task<IEnumerable<MessageVm>> Handle(GetAllMessagesQuery request, CancellationToken cancellationToken) {
		var userId = currentUserService.GetRequiredUserId();

		var items = await context.Messages
			.AsNoTracking()
			.Where(m => m.Chat.CustomerId == userId || m.Chat.RealtorId == userId)
			.Where(m => m.ChatId == request.ChatId)
			.OrderBy(m => m.CreatedAtUtc)
			.ProjectTo<MessageVm>(mapper.ConfigurationProvider)
			.ToArrayAsync(cancellationToken);

		return items;
	}
}
