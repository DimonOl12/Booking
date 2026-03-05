using EasyStay.Application.MediatR.Chats.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.Chats.Queries.GetAll;

public class GetAllChatsQuery : IRequest<IEnumerable<ChatVm>> { }
