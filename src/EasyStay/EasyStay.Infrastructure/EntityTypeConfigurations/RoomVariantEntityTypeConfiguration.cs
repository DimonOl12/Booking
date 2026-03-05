using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class RoomVariantEntityTypeConfiguration : IEntityTypeConfiguration<RoomVariant> {
	public void Configure(EntityTypeBuilder<RoomVariant> builder) {
		builder.ToTable("RoomVariants");
	}
}
