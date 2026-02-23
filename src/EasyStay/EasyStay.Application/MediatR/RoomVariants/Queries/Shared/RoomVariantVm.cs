using EasyStay.Application.Common.Mappings;
using EasyStay.Application.MediatR.BedInfos.Queries.Shared;
using EasyStay.Application.MediatR.GuestInfos.Queries.Shared;
using EasyStay.Domain.Entities;

namespace EasyStay.Application.MediatR.RoomVariants.Queries.Shared;

public class RoomVariantVm : IMapWith<RoomVariant> {
	public long Id { get; set; }

	public decimal Price { get; set; }

	public decimal? DiscountPrice { get; set; }

	public long RoomId { get; set; }

	public GuestInfoVm GuestInfo { get; set; } = null!;

	public BedInfoVm BedInfo { get; set; } = null!;
}
