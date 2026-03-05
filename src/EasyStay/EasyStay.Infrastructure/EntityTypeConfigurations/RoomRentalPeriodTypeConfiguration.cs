using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class RoomRentalPeriodTypeConfiguration : IEntityTypeConfiguration<RoomRentalPeriod> {
	public void Configure(EntityTypeBuilder<RoomRentalPeriod> builder) {
		builder.ToTable("RoomRentalPeriods");

		builder.HasKey(rrp => new {
			rrp.RoomId,
			rrp.RentalPeriodId
		});
	}
}
