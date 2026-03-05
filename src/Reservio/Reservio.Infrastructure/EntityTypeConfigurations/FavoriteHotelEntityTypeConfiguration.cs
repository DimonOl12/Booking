using Reservio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reservio.Infrastructure.EntityTypeConfigurations;

internal class FavoriteHotelEntityTypeConfiguration : IEntityTypeConfiguration<FavoriteHotel> {
	public void Configure(EntityTypeBuilder<FavoriteHotel> builder) {
		builder.ToTable("FavoriteHotels");

		builder.HasKey(fh => new {
			fh.HotelId,
			fh.CustomerId
		});
	}
}

