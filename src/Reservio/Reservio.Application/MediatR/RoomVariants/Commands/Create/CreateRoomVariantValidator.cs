using Reservio.Application.Interfaces;
using FluentValidation;

namespace Reservio.Application.MediatR.RoomVariants.Commands.Create;

public class CreateRoomVariantValidator : AbstractValidator<CreateRoomVariantCommand> {
	public CreateRoomVariantValidator(IExistingEntityCheckerService existingEntityCheckerService) {
		RuleFor(rv => rv.Price)
			.GreaterThanOrEqualTo(0)
				.WithMessage("Price must be greater than or equal to 0.");

		RuleFor(rv => rv.DiscountPrice)
			.GreaterThanOrEqualTo(0)
				.WithMessage("Discount price must be greater than or equal to 0.");

		RuleFor(rv => rv.RoomId)
			.MustAsync(existingEntityCheckerService.IsCorrectRoomIdOfCurrentUserAsync)
				.WithMessage("Room does not exist or is not yours.");
	}
}

