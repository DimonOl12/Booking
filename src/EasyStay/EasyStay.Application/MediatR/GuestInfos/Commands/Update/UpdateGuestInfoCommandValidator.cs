using FluentValidation;

namespace EasyStay.Application.MediatR.GuestInfos.Commands.Update;

public class UpdateGuestInfoCommandValidator : AbstractValidator<UpdateGuestInfoCommand> {
	public UpdateGuestInfoCommandValidator() {
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
