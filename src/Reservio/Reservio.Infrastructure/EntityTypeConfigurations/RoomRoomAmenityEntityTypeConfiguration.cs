using Reservio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Reservio.Infrastructure.EntityTypeConfigurations;

internal class RoomRoomAmenityEntityTypeConfiguration : IEntityTypeConfiguration<RoomRoomAmenity> {
	public void Configure(EntityTypeBuilder<RoomRoomAmenity> builder) {
		builder.ToTable("RoomRoomAmenities");

		builder.HasKey(rt => new {
			rt.RoomId,
			rt.RoomAmenityId
		});
	}
}

