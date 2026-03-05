using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class HotelBreakfastEntityTypeConfiguration : IEntityTypeConfiguration<HotelBreakfast> {
	public void Configure(EntityTypeBuilder<HotelBreakfast> builder) {
		builder.ToTable("HotelBreakfasts");

		builder.HasKey(hb => new {
			hb.HotelId,
			hb.BreakfastId
		});
	}
}
