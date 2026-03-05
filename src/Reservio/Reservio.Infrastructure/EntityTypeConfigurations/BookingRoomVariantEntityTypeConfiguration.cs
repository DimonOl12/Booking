using Reservio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reservio.Infrastructure.EntityTypeConfigurations;

internal class BookingRoomVariantEntityTypeConfiguration : IEntityTypeConfiguration<BookingRoomVariant> {
	public void Configure(EntityTypeBuilder<BookingRoomVariant> builder) {
		builder.ToTable("BookingRoomVariants");
	}
}

