using Reservio.Domain.Entities.Identity;
using Reservio.Domain.EntityInterfaces;

namespace Reservio.Domain.Entities;

public class RealtorReview : ITimestamped {
	public long Id { get; set; }

	public string Description { get; set; } = null!;

	public double? Score { get; set; }

	public DateTime CreatedAtUtc { get; set; }

	public DateTime? UpdatedAtUtc { get; set; }

	public long AuthorId;
	public Customer Author { get; set; } = null!;

	public long RealtorId { get; set; }
	public Realtor Realtor { get; set; } = null!;
}

