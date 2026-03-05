using Reservio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reservio.Infrastructure.EntityTypeConfigurations;

internal class GenderEntityTypeConfiguration : IEntityTypeConfiguration<Gender> {
	public void Configure(EntityTypeBuilder<Gender> builder) {
		builder.ToTable("Genders");

		builder.Property(g => g.Name)
			.HasMaxLength(255)
			.IsRequired();
	}
}

