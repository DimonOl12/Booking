using FluentValidation;

namespace EasyStay.Application.MediatR.BedInfos.Commands.Update;

public class UpdateBedInfoCommandValidator : AbstractValidator<UpdateBedInfoCommand> {
	public UpdateBedInfoCommandValidator() {
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
