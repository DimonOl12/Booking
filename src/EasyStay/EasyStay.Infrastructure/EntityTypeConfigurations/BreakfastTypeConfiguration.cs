using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class BreakfastTypeConfiguration : IEntityTypeConfiguration<Breakfast> {
	public void Configure(EntityTypeBuilder<Breakfast> builder) {
		builder.ToTable("Breakfasts");

		builder.Property(b => b.Name)
			.HasMaxLength(255)
			.IsRequired();
	}
}
