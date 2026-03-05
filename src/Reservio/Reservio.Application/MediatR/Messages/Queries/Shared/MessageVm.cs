using Reservio.Application.Common.Mappings;
using Reservio.Domain.Entities;

namespace Reservio.Application.MediatR.Messages.Queries.Shared;

public class MessageVm : IMapWith<Message> {
	public long Id { get; set; }

	public string Text { get; set; } = null!;

	public DateTime CreatedAtUtc { get; set; }

	public DateTime? UpdatedAtUtc { get; set; }

	public long ChatId { get; set; }

	public long AuthorId { get; set; }
}

