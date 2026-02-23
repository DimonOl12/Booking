using EasyStay.Application.Interfaces;
using FluentValidation;

namespace EasyStay.Application.MediatR.GuestInfos.Commands.Create;

public class CreateGuestInfoCommandValidator : AbstractValidator<CreateGuestInfoCommand> {
	public CreateGuestInfoCommandValidator(IExistingEntityCheckerService existingEntityCheckerService) {
		RuleFor(gi => gi.RoomVariantId)
			.MustAsync(existingEntityCheckerService.IsCorrectRoomVariantIdOfCurrentUserAsync)
				.WithMessage("RoomVariant with this Id doesn't exist.");

		RuleFor(gi => gi.AdultCount)
			.GreaterThanOrEqualTo(0)
				.WithMessage("AdultCount must be greater or equal to 0.");

		RuleFor(gi => gi.ChildCount)
			.GreaterThanOrEqualTo(0)
				.WithMessage("ChildCount must be greater or equal to 0.");

		RuleFor(gi => gi)
			.Must(gi => gi.AdultCount + gi.ChildCount > 0)
				.WithMessage("AdultCount and ChildCount must be greater than 0.");
	}
}
