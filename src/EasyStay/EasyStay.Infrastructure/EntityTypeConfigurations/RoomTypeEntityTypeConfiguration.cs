using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class RoomTypeEntityTypeConfiguration : IEntityTypeConfiguration<RoomType> {
	public void Configure(EntityTypeBuilder<RoomType> builder) {
		builder.ToTable("RoomTypes");

		builder.Property(rt => rt.Name)
			.HasMaxLength(255)
			.IsRequired();
	}
}
