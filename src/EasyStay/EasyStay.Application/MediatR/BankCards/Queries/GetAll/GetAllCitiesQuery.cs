using EasyStay.Application.MediatR.BankCards.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.BankCards.Queries.GetAll;

public class GetAllBankCardsQuery : IRequest<IEnumerable<BankCardVm>> { }
