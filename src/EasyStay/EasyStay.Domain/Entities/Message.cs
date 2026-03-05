using EasyStay.Domain.Entities.Identity;
using EasyStay.Domain.EntityInterfaces;

namespace EasyStay.Domain.Entities;

public class Message : ITimestamped {
	public long Id { get; set; }

	public string Text { get; set; } = null!;

	public DateTime CreatedAtUtc { get; set; }

	public DateTime? UpdatedAtUtc { get; set; }

	public long ChatId { get; set; }
	public Chat Chat { get; set; } = null!;

	public long AuthorId { get; set; }
	public User Author { get; set; } = null!;
}
