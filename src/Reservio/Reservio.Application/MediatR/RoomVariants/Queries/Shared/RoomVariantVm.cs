using Reservio.Application.Common.Mappings;
using Reservio.Application.MediatR.BedInfos.Queries.Shared;
using Reservio.Application.MediatR.GuestInfos.Queries.Shared;
using Reservio.Domain.Entities;

namespace Reservio.Application.MediatR.RoomVariants.Queries.Shared;

public class RoomVariantVm : IMapWith<RoomVariant> {
	public long Id { get; set; }

	public decimal Price { get; set; }

	public decimal? DiscountPrice { get; set; }

	public long RoomId { get; set; }

	public GuestInfoVm GuestInfo { get; set; } = null!;

	public BedInfoVm BedInfo { get; set; } = null!;
}

