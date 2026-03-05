using Reservio.Application.Interfaces;
using FluentValidation;

namespace Reservio.Application.MediatR.RoomVariants.Commands.Update;

public class UpdateRoomVariantValidator : AbstractValidator<UpdateRoomVariantCommand> {
	public UpdateRoomVariantValidator(IExistingEntityCheckerService existingEntityCheckerService) {
		RuleFor(rv => rv.Price)
			.GreaterThanOrEqualTo(0)
				.WithMessage("Price must be greater than or equal to 0.");

		RuleFor(rv => rv.DiscountPrice)
			.GreaterThanOrEqualTo(0)
				.WithMessage("Discount price must be greater than or equal to 0.");
	}
}

