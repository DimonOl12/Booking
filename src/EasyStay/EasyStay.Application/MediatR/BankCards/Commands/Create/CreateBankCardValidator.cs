using EasyStay.Application.Interfaces;
using FluentValidation;

namespace EasyStay.Application.MediatR.BankCards.Commands.Create;

public class CreateBankCardValidator : AbstractValidator<CreateBankCardCommand> {
	public CreateBankCardValidator(IDateConverter dateConverter) {
		RuleFor(bc => bc.Number)
			.NotEmpty()
				.WithMessage("Card number is required")
			.MaximumLength(16)
				.WithMessage("Card number is too long")
			.CreditCard()
				.WithMessage("Card number is not valid");

		RuleFor(bc => bc.ExpirationDate)
			.GreaterThanOrEqualTo(dateConverter.ToFirstDayOfMonth(DateOnly.FromDateTime(DateTime.Now)))
				.WithMessage("Image is not valid");

		RuleFor(bc => bc.Cvv)
			.NotEmpty()
				.WithMessage("Cvv is required")
			.MaximumLength(3)
				.WithMessage("Cvv is too long")
			.Must(cvv => cvv.Length == 3)
				.WithMessage("Cvv length is not valid")
			.Must(cvv => int.TryParse(cvv, out _))
				.WithMessage("Cvv is not a number");

		RuleFor(bc => bc.OwnerFullName)
			.NotEmpty()
				.WithMessage("Owner full name is required")
			.MaximumLength(255)
				.WithMessage("Owner full name is too long");
	}
}
