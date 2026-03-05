using FluentValidation;

namespace EasyStay.Application.MediatR.RentalPeriods.Commands.Update;

public class UpdateRentalPeriodValidator : AbstractValidator<UpdateRentalPeriodCommand> {
	public UpdateRentalPeriodValidator() {
		RuleFor(rp => rp.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null")
			.MaximumLength(255)
				.WithMessage("Name is too long");
	}
}
