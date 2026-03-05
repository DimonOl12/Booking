using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class BookingRoomVariantEntityTypeConfiguration : IEntityTypeConfiguration<BookingRoomVariant> {
	public void Configure(EntityTypeBuilder<BookingRoomVariant> builder) {
		builder.ToTable("BookingRoomVariants");
	}
}
