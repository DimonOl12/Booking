using Reservio.Application.Models.BedInfos;
using Reservio.Application.Models.GuestInfos;
using MediatR;

namespace Reservio.Application.MediatR.RoomVariants.Commands.Create;

public class CreateRoomVariantCommand : IRequest<long> {
	public decimal Price { get; set; }

	public decimal? DiscountPrice { get; set; }

	public long RoomId { get; set; }

	public CreateUpdateGuestInfoDto GuestInfo { get; set; } = null!;

	public CreateUpdateBedInfoDto BedInfo { get; set; } = null!;
}

