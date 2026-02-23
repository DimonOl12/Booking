using EasyStay.Domain.Entities.Identity;

namespace EasyStay.Domain.Entities;

public class Chat {
	public long Id { get; set; }

	public long CustomerId { get; set; }
	public Customer Customer { get; set; } = null!;

	public long RealtorId { get; set; }
	public Realtor Realtor { get; set; } = null!;

	public ICollection<Message> Messages { get; set; } = null!;
}
