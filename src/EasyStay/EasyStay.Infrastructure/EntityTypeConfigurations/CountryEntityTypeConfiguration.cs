using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class CountryEntityTypeConfiguration : IEntityTypeConfiguration<Country> {
	public void Configure(EntityTypeBuilder<Country> builder) {
		builder.ToTable("Countries");

		builder.Property(c => c.Name)
			.HasMaxLength(255)
			.IsRequired();

		builder.Property(c => c.Image)
			.HasMaxLength(255)
			.IsRequired();
	}
}
