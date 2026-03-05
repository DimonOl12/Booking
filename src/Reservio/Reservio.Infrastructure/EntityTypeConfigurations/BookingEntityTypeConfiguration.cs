using Reservio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reservio.Infrastructure.EntityTypeConfigurations;

internal class BookingEntityTypeConfiguration : IEntityTypeConfiguration<Booking> {
	public void Configure(EntityTypeBuilder<Booking> builder) {
		builder.ToTable("Bookings");

		builder.Property(b => b.PersonalWishes)
			.HasMaxLength(4000)
			.IsRequired(false);
	}
}

