using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class RoomEntityTypeConfiguration : IEntityTypeConfiguration<Room> {
	public void Configure(EntityTypeBuilder<Room> builder) {
		builder.ToTable("Rooms");

		builder.Property(r => r.Name)
			.HasMaxLength(255)
			.IsRequired();
	}
}
