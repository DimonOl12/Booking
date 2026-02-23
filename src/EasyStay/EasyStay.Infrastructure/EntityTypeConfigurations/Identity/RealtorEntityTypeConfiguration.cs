using EasyStay.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations.Identity;

internal class RealtorEntityTypeConfiguration : IEntityTypeConfiguration<Realtor> {
	public void Configure(EntityTypeBuilder<Realtor> builder) {
		builder.ToTable("Realtors");

		builder.Property(r => r.Description)
			.HasMaxLength(4000)
			.IsRequired(false);

		builder.Property(r => r.DateOfBirth)
			.IsRequired(false);

		builder.Property(r => r.Address)
			.HasMaxLength(255)
			.IsRequired(false);
	}
}
