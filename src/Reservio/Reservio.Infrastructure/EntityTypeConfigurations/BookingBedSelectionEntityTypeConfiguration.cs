using Reservio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reservio.Infrastructure.EntityTypeConfigurations;

internal class BookingBedSelectionEntityTypeConfiguration : IEntityTypeConfiguration<BookingBedSelection> {
	public void Configure(EntityTypeBuilder<BookingBedSelection> builder) {
		builder.ToTable("BookingBedSelections");

		builder.HasKey(bbs => bbs.BookingRoomVariantId);
	}
}

