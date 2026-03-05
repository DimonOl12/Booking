using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class CitizenshipEntityTypeConfiguration : IEntityTypeConfiguration<Citizenship> {
	public void Configure(EntityTypeBuilder<Citizenship> builder) {
		builder.ToTable("Citizenships");

		builder.Property(c => c.Name)
			.HasMaxLength(255)
			.IsRequired();
	}
}
