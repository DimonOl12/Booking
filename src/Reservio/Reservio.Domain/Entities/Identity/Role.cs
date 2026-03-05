using Microsoft.AspNetCore.Identity;

namespace Reservio.Domain.Entities.Identity;

public class Role : IdentityRole<long> {
	public virtual ICollection<UserRole> UserRoles { get; set; } = null!;
}

