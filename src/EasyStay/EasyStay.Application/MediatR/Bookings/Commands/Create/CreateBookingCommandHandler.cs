using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Bookings.Commands.Create;

public class CreateBookingCommandHandler(
	IEasyStayDbContext context,
	ITimeConverter timeConverter,
	ICurrentUserService currentUserService,
	IMediator mediator
) : IRequestHandler<CreateBookingCommand, long> {

	public async Task<long> Handle(CreateBookingCommand request, CancellationToken cancellationToken) {
		var roomVariants = await context.RoomVariants
		   .Where(
			   rv => request.BookingRoomVariants
				   .Select(brv => brv.RoomVariantId)
				   .Contains(rv.Id)
		   )
		   .ToArrayAsync(cancellationToken);

		var amountToPay = roomVariants
			.Sum(
				rv => (rv.DiscountPrice ?? rv.Price) *
					request.BookingRoomVariants.First(brv => brv.RoomVariantId == rv.Id).Quantity
			);

		var entity = new Booking {
			DateFrom = request.DateFrom,
			DateTo = request.DateTo,
			PersonalWishes = request.PersonalWishes,
			EstimatedTimeOfArrivalUtc = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(request.EstimatedTimeOfArrivalUtc),
			AmountToPay = amountToPay,
			CustomerId = currentUserService.GetRequiredUserId()
		};

		using var transaction = await context.BeginTransactionAsync(cancellationToken);
		try {
			if (request.BankCardId is not null)
				entity.BankCardId = request.BankCardId;
			else if (request.BankCard is not null)
				entity.BankCardId = await mediator.Send(request.BankCard, cancellationToken);

			entity.BookingRoomVariants = request.BookingRoomVariants
				.Select(brv => new BookingRoomVariant {
					Quantity = brv.Quantity,
					RoomVariantId = brv.RoomVariantId,
					Booking = entity,
					BookingBedSelection = new BookingBedSelection {
						IsSingleBed = brv.BookingBedSelection.IsSingleBed,
						IsDoubleBed = brv.BookingBedSelection.IsDoubleBed,
						IsExtraBed = brv.BookingBedSelection.IsExtraBed,
						IsSofa = brv.BookingBedSelection.IsSofa,
						IsKingsizeBed = brv.BookingBedSelection.IsKingsizeBed
					}
				})
				.ToArray();

			await context.Bookings.AddAsync(entity, cancellationToken);
			await context.SaveChangesAsync(cancellationToken);

			await transaction.CommitAsync(cancellationToken);
		}
		catch {
			await transaction.RollbackAsync(cancellationToken);
			throw;
		}

		return entity.Id;
	}
}
