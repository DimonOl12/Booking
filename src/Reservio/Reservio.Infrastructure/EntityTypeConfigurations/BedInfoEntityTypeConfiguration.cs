using Reservio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reservio.Infrastructure.EntityTypeConfigurations;

internal class BedInfoEntityTypeConfiguration : IEntityTypeConfiguration<BedInfo> {
	public void Configure(EntityTypeBuilder<BedInfo> builder) {
		builder.ToTable("BedInfos");

		builder.HasKey(bi => bi.RoomVariantId);
	}
}

