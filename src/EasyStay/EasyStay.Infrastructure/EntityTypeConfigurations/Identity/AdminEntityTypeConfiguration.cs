using EasyStay.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations.Identity;

internal class AdminEntityTypeConfiguration : IEntityTypeConfiguration<Admin> {
	public void Configure(EntityTypeBuilder<Admin> builder) {
		builder.ToTable("Admins");
	}
}
