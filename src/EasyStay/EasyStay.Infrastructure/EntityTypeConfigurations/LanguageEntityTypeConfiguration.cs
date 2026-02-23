using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class LanguageEntityTypeConfiguration : IEntityTypeConfiguration<Language> {
	public void Configure(EntityTypeBuilder<Language> builder) {
		builder.ToTable("Languages");

		builder.Property(l => l.Name)
			.HasMaxLength(255)
			.IsRequired();

		builder.HasIndex(l => l.Name)
			.IsUnique();
	}
}
