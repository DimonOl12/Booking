using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class HotelHotelAmenityEntityTypeConfiguration : IEntityTypeConfiguration<HotelHotelAmenity> {
	public void Configure(EntityTypeBuilder<HotelHotelAmenity> builder) {
		builder.ToTable("HotelHotelAmenities");

		builder.HasKey(hha => new {
			hha.HotelId,
			hha.HotelAmenityId
		});
	}
}
