using EasyStay.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations.Identity;

internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User> {
	public void Configure(EntityTypeBuilder<User> builder) {
		builder.UseTptMappingStrategy();

		builder.Property(u => u.FirstName)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(u => u.LastName)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(u => u.Photo)
			.IsRequired()
			.HasMaxLength(200);
	}
}
