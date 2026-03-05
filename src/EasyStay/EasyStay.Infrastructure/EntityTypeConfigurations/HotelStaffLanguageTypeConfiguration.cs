using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class HotelStaffLanguageTypeConfiguration : IEntityTypeConfiguration<HotelStaffLanguage> {
	public void Configure(EntityTypeBuilder<HotelStaffLanguage> builder) {
		builder.ToTable("HotelStaffLanguages");

		builder.HasKey(hsl => new {
			hsl.HotelId,
			hsl.LanguageId
		});
	}
}
