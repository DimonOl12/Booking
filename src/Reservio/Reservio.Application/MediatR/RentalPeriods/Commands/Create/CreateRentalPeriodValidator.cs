using FluentValidation;

namespace Reservio.Application.MediatR.RentalPeriods.Commands.Create;

public class CreateRentalPeriodValidator : AbstractValidator<CreateRentalPeriodCommand> {
	public CreateRentalPeriodValidator() {
		RuleFor(rp => rp.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null")
			.MaximumLength(255)
				.WithMessage("Name is too long");
	}
}

