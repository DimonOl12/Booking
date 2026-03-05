using EasyStay.Application.Interfaces;
using FluentValidation;

namespace EasyStay.Application.MediatR.BedInfos.Commands.Create;

public class CreateBedInfoCommandValidator : AbstractValidator<CreateBedInfoCommand> {
	public CreateBedInfoCommandValidator(IExistingEntityCheckerService existingEntityCheckerService) {
		RuleFor(bi => bi.RoomVariantId)
			.MustAsync(existingEntityCheckerService.IsCorrectRoomVariantIdOfCurrentUserAsync)
				.WithMessage("RoomVariant is not exist or not yours");

		RuleFor(bi => bi.SingleBedCount)
			.GreaterThanOrEqualTo(0)
				.WithMessage("SingleBedCount must be greater than or equal to 0");

		RuleFor(bi => bi.DoubleBedCount)
			.GreaterThanOrEqualTo(0)
				.WithMessage("DoubleBedCount must be greater than or equal to 0");

		RuleFor(bi => bi.ExtraBedCount)
			.GreaterThanOrEqualTo(0)
				.WithMessage("ExtraBedCount must be greater than or equal to 0");

		RuleFor(bi => bi.SofaCount)
			.GreaterThanOrEqualTo(0)
				.WithMessage("SofaCount must be greater than or equal to 0");

		RuleFor(bi => bi.KingsizeBedCount)
			.GreaterThanOrEqualTo(0)
				.WithMessage("KingsizeBedCount must be greater than or equal to 0");
	}
}
