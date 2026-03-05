using Reservio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reservio.Infrastructure.EntityTypeConfigurations;

internal class HotelAmenityEntityTypeConfiguration : IEntityTypeConfiguration<HotelAmenity> {
	public void Configure(EntityTypeBuilder<HotelAmenity> builder) {
		builder.ToTable("HotelAmenities");

		builder.Property(ha => ha.Name)
			.HasMaxLength(255)
			.IsRequired();

		builder.Property(ha => ha.Image)
			.HasMaxLength(255)
			.IsRequired();
	}
}

