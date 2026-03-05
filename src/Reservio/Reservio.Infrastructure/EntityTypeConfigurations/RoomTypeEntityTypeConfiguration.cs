using Reservio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reservio.Infrastructure.EntityTypeConfigurations;

internal class RoomTypeEntityTypeConfiguration : IEntityTypeConfiguration<RoomType> {
	public void Configure(EntityTypeBuilder<RoomType> builder) {
		builder.ToTable("RoomTypes");

		builder.Property(rt => rt.Name)
			.HasMaxLength(255)
			.IsRequired();
	}
}

