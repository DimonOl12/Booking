using Microsoft.AspNetCore.Identity;

namespace EasyStay.Domain.Entities.Identity;

public class Role : IdentityRole<long> {
	public virtual ICollection<UserRole> UserRoles { get; set; } = null!;
}
