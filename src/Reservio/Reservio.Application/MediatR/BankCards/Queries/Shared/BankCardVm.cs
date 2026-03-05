using Reservio.Application.Common.Mappings;
using Reservio.Domain.Entities;

namespace Reservio.Application.MediatR.BankCards.Queries.Shared;

public class BankCardVm : IMapWith<BankCard> {
	public long Id { get; set; }

	public string Number { get; set; } = null!;

	public DateOnly ExpirationDate { get; set; }

	public string Cvv { get; set; } = null!;

	public string OwnerFullName { get; set; } = null!;
}

