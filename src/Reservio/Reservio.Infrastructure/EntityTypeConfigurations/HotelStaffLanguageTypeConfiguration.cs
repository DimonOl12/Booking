using Reservio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reservio.Infrastructure.EntityTypeConfigurations;

internal class HotelStaffLanguageTypeConfiguration : IEntityTypeConfiguration<HotelStaffLanguage> {
	public void Configure(EntityTypeBuilder<HotelStaffLanguage> builder) {
		builder.ToTable("HotelStaffLanguages");

		builder.HasKey(hsl => new {
			hsl.HotelId,
			hsl.LanguageId
		});
	}
}

