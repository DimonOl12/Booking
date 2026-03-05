using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class FavoriteHotelEntityTypeConfiguration : IEntityTypeConfiguration<FavoriteHotel> {
	public void Configure(EntityTypeBuilder<FavoriteHotel> builder) {
		builder.ToTable("FavoriteHotels");

		builder.HasKey(fh => new {
			fh.HotelId,
			fh.CustomerId
		});
	}
}
