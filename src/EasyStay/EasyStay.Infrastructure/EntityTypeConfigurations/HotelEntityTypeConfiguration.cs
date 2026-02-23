using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class HotelEntityTypeConfiguration : IEntityTypeConfiguration<Hotel> {
	public void Configure(EntityTypeBuilder<Hotel> builder) {
		builder.ToTable("Hotels");

		builder.Property(h => h.Name)
			.HasMaxLength(255)
			.IsRequired();

		builder.Property(h => h.Description)
			.HasMaxLength(4000)
			.IsRequired();
	}
}
