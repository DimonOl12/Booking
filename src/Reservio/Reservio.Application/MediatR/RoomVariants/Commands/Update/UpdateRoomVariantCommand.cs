using Reservio.Application.Models.BedInfos;
using Reservio.Application.Models.GuestInfos;
using MediatR;

namespace Reservio.Application.MediatR.RoomVariants.Commands.Update;

public class UpdateRoomVariantCommand : IRequest {
	public long Id { get; set; }

	public decimal Price { get; set; }

	public decimal? DiscountPrice { get; set; }

	public CreateUpdateGuestInfoDto GuestInfo { get; set; } = null!;

	public CreateUpdateBedInfoDto BedInfo { get; set; } = null!;
}

