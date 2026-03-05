using EasyStay.Application.MediatR.BankCards.Commands.Create;
using EasyStay.Application.Models.BookingRoomVariants;
using MediatR;

namespace EasyStay.Application.MediatR.Bookings.Commands.Create;

public class CreateBookingCommand : IRequest<long> {
	public DateOnly DateFrom { get; set; }

	public DateOnly DateTo { get; set; }

	public string? PersonalWishes { get; set; }

	public TimeOnly EstimatedTimeOfArrivalUtc { get; set; }

	public long? BankCardId { get; set; }

	public CreateBankCardCommand? BankCard { get; set; }

	public IEnumerable<CreateBookingRoomVariantDto> BookingRoomVariants { get; set; } = null!;
}
