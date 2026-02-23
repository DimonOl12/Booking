using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class GenderEntityTypeConfiguration : IEntityTypeConfiguration<Gender> {
	public void Configure(EntityTypeBuilder<Gender> builder) {
		builder.ToTable("Genders");

		builder.Property(g => g.Name)
			.HasMaxLength(255)
			.IsRequired();
	}
}
