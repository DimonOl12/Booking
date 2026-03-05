using Reservio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reservio.Infrastructure.EntityTypeConfigurations;

internal class HotelHotelAmenityEntityTypeConfiguration : IEntityTypeConfiguration<HotelHotelAmenity> {
	public void Configure(EntityTypeBuilder<HotelHotelAmenity> builder) {
		builder.ToTable("HotelHotelAmenities");

		builder.HasKey(hha => new {
			hha.HotelId,
			hha.HotelAmenityId
		});
	}
}

