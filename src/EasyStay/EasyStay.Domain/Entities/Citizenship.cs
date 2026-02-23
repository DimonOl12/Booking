using EasyStay.Domain.Entities.Identity;

namespace EasyStay.Domain.Entities;

public class Citizenship {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public ICollection<Realtor> Realtors { get; set; } = null!;
}
