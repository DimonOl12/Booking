using EasyStay.Application.Models.Date;
using MediatR;

namespace EasyStay.Application.MediatR.Rooms.Queries.GetRoomVariantsFreeQuantity;

public class GetRoomVariantsFreeQuantityQuery : IRequest<int> {
	public long Id { get; set; }

	public FreeDatePeriod FreePeriod { get; set; } = null!;
}
