using Reservio.Domain.Entities.Identity;

namespace Reservio.Domain.Entities;

public class Citizenship {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public ICollection<Realtor> Realtors { get; set; } = null!;
}

