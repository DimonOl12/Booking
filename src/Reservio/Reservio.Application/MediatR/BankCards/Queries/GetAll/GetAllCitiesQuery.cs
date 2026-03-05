using Reservio.Application.MediatR.BankCards.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.BankCards.Queries.GetAll;

public class GetAllBankCardsQuery : IRequest<IEnumerable<BankCardVm>> { }

