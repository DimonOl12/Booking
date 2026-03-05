using Reservio.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reservio.Infrastructure.EntityTypeConfigurations.Identity;

internal class AdminEntityTypeConfiguration : IEntityTypeConfiguration<Admin> {
	public void Configure(EntityTypeBuilder<Admin> builder) {
		builder.ToTable("Admins");
	}
}

