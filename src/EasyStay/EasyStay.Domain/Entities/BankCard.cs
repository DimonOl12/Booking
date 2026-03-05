using EasyStay.Domain.Entities.Identity;

namespace EasyStay.Domain.Entities;

public class BankCard {
	public long Id { get; set; }

	public string Number { get; set; } = null!;

	public DateOnly ExpirationDate { get; set; }

	public string Cvv { get; set; } = null!;

	public string OwnerFullName { get; set; } = null!;

	public long? CustomerId { get; set; }
	public Customer? Customer { get; set; }

	public ICollection<Booking> Bookings { get; set; } = null!;
}
