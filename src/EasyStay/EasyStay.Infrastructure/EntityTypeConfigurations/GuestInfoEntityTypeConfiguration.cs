using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyStay.Infrastructure.EntityTypeConfigurations;

internal class GuestInfoEntityTypeConfiguration : IEntityTypeConfiguration<GuestInfo> {
	public void Configure(EntityTypeBuilder<GuestInfo> builder) {
		builder.ToTable("GuestInfos");

		builder.HasKey(gi => gi.RoomVariantId);
	}
}
