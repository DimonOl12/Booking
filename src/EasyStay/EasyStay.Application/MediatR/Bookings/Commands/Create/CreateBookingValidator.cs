using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.BankCards.Commands.Create;
using EasyStay.Application.Models.BookingRoomVariants;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Bookings.Commands.Create;

public class CreateBookingValidator : AbstractValidator<CreateBookingCommand> {
	private readonly IEasyStayDbContext _context;

	public CreateBookingValidator(IValidator<CreateBankCardCommand> bankCardValidator, IExistingEntityCheckerService existingEntityCheckerService, IEasyStayDbContext context, ICollectionValidator collectionValidator) {
		_context = context;

		RuleFor(b => b.DateTo)
			.GreaterThanOrEqualTo(b => b.DateFrom)
				.WithMessage("Dates form an incorrect time segment.");

		RuleFor(b => b.PersonalWishes)
			.MaximumLength(4000)
				.WithMessage("Personal wishes is too long.");

		RuleFor(b => b)
			.Must(b => !(b.BankCardId is not null && b.BankCard is not null))
				.WithMessage("Only one way to provide bank card information.");

		RuleFor(b => b.BankCardId)
			.MustAsync(async (long? bcId, CancellationToken cancellationToken) => {
				if (bcId is null)
					return true;

				return await existingEntityCheckerService.IsCorrectBankCardIdOfCurrentUserAsync(bcId.Value, cancellationToken);
			})
				.WithMessage("Incorrect bank card id.");

		RuleFor(b => b.BankCard)
			.MustAsync(async (CreateBankCardCommand? bc, CancellationToken cancellationToken) => {
				if (bc is null)
					return true;

				return (await bankCardValidator.ValidateAsync(bc, cancellationToken)).IsValid;
			})
				.WithMessage("Incorrect bank card information.");

		void validateBookingRoomVariantsIfRoomVariantIdsIsUnique() {
			RuleFor(b => b.BookingRoomVariants)
				.MustAsync(async (IEnumerable<CreateBookingRoomVariantDto> brvs, CancellationToken cancellationToken) =>
					await existingEntityCheckerService.IsCorrectRoomVariantIdsAsync(
						brvs.Select(brv => brv.RoomVariantId),
						cancellationToken
					)
				)
					.WithMessage("There are booking room variant that do not exist.")
				.MustAsync(IsRoomVariantsFromSameRoomAsync)
					.WithMessage("Room variants must be from the same room.")
					.DependentRules(() => {
						RuleForEach(b => b.BookingRoomVariants)
							.MustAsync(IsValidBedSelectionAsync)
								.WithMessage("There is a selected bed type that is not available in the room.");

						RuleFor(b => b)
							.MustAsync(IsValidRequestedNumberOfRoomsAsync)
								.WithMessage("The requested number of rooms exceeds the available number of rooms.");
					});
		}

		RuleFor(b => b.BookingRoomVariants)
			.Must(brvs => collectionValidator.IsDistinct(brvs.Select(brv => brv.RoomVariantId)))
				.WithMessage("Booking room variants must be unique.")
				.DependentRules(validateBookingRoomVariantsIfRoomVariantIdsIsUnique);

		RuleForEach(b => b.BookingRoomVariants)
			.Must(brv => {
				var bedSelection = brv.BookingBedSelection;

				return bedSelection.IsSingleBed || bedSelection.IsDoubleBed || bedSelection.IsExtraBed
					|| bedSelection.IsSofa || bedSelection.IsKingsizeBed;
			})
				.WithMessage("Must be selected at least one bed type.")
			.Must(brv => brv.Quantity > 0)
				.WithMessage("Quantity must be greater than 0.");
	}



	private async Task<bool> IsRoomVariantsFromSameRoomAsync(IEnumerable<CreateBookingRoomVariantDto> brvs, CancellationToken cancellationToken) {
		var rvIds = brvs.Select(brv => brv.RoomVariantId).ToArray();

		return (await _context.RoomVariants
			.Where(rv => rvIds.Contains(rv.Id))
			.Select(rv => rv.RoomId)
			.Distinct()
			.CountAsync(cancellationToken)) == 1;
	}

	private async Task<bool> IsValidRequestedNumberOfRoomsAsync(CreateBookingCommand b, CancellationToken cancellationToken) {
		var anyRoomVariantId = b.BookingRoomVariants.First().RoomVariantId;

		var roomQuantity = await _context.Rooms
			.Where(r => r.RoomVariants.Any(rv => rv.Id == anyRoomVariantId))
			.Select(r => r.Quantity)
			.FirstAsync(cancellationToken);

		var quantityOfRentedRooms = await _context.BookingRoomVariants
			.Where(brv => b.BookingRoomVariants.Select(b => b.RoomVariantId).Contains(brv.RoomVariantId))
			.Where(brv => (b.DateFrom <= brv.Booking.DateTo) && (b.DateTo >= brv.Booking.DateFrom))
			.SumAsync(brv => brv.Quantity, cancellationToken);

		var roomsAvailable = roomQuantity - quantityOfRentedRooms;
		var roomsRequested = b.BookingRoomVariants.Sum(brv => brv.Quantity);

		return roomsAvailable >= roomsRequested;
	}

	private async Task<bool> IsValidBedSelectionAsync(CreateBookingRoomVariantDto brv, CancellationToken cancellationToken) {
		var bedSelection = brv.BookingBedSelection;
		var bedInfo = await _context.BedInfos
			.AsNoTracking()
			.FirstAsync(bi => bi.RoomVariantId == brv.RoomVariantId, cancellationToken);

		return bedInfo.SingleBedCount >= Convert.ToInt32(bedSelection.IsSingleBed)
			&& bedInfo.DoubleBedCount >= Convert.ToInt32(bedSelection.IsDoubleBed)
			&& bedInfo.ExtraBedCount >= Convert.ToInt32(bedSelection.IsExtraBed)
			&& bedInfo.SofaCount >= Convert.ToInt32(bedSelection.IsSofa)
			&& bedInfo.KingsizeBedCount >= Convert.ToInt32(bedSelection.IsKingsizeBed);
	}
}
