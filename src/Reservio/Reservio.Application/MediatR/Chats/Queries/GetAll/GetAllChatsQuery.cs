using Reservio.Application.MediatR.Chats.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.Chats.Queries.GetAll;

public class GetAllChatsQuery : IRequest<IEnumerable<ChatVm>> { }

