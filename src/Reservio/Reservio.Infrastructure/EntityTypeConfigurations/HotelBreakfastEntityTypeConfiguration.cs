using Reservio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reservio.Infrastructure.EntityTypeConfigurations;

internal class HotelBreakfastEntityTypeConfiguration : IEntityTypeConfiguration<HotelBreakfast> {
	public void Configure(EntityTypeBuilder<HotelBreakfast> builder) {
		builder.ToTable("HotelBreakfasts");

		builder.HasKey(hb => new {
			hb.HotelId,
			hb.BreakfastId
		});
	}
}

