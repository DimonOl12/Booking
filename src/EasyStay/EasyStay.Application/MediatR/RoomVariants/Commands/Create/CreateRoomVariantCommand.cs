using EasyStay.Application.Models.BedInfos;
using EasyStay.Application.Models.GuestInfos;
using MediatR;

namespace EasyStay.Application.MediatR.RoomVariants.Commands.Create;

public class CreateRoomVariantCommand : IRequest<long> {
	public decimal Price { get; set; }

	public decimal? DiscountPrice { get; set; }

	public long RoomId { get; set; }

	public CreateUpdateGuestInfoDto GuestInfo { get; set; } = null!;

	public CreateUpdateBedInfoDto BedInfo { get; set; } = null!;
}
