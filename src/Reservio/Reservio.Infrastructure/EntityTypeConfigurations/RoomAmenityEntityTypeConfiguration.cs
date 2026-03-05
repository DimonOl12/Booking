using Reservio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reservio.Infrastructure.EntityTypeConfigurations;

internal class RoomAmenityEntityTypeConfiguration : IEntityTypeConfiguration<RoomAmenity> {
	public void Configure(EntityTypeBuilder<RoomAmenity> builder) {
		builder.ToTable("RoomAmenities");

		builder.Property(ra => ra.Name)
			.HasMaxLength(255)
			.IsRequired();
	}
}

