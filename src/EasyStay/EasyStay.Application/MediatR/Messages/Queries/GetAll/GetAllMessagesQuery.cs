using EasyStay.Application.MediatR.Messages.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.Messages.Queries.GetAll;

public class GetAllMessagesQuery : IRequest<IEnumerable<MessageVm>> {
	public long ChatId { get; set; }
}
