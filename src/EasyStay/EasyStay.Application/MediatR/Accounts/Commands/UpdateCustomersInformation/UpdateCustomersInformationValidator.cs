using EasyStay.Application.Interfaces;
using FluentValidation;

namespace EasyStay.Application.MediatR.Accounts.Commands.UpdateCustomersInformation;

public class UpdateCustomersInformationValidator : AbstractValidator<UpdateCustomersInformationCommand> {
	public UpdateCustomersInformationValidator(IExistingEntityCheckerService existingEntityCheckerService) {
		RuleFor(r => r.PhoneNumber)
			.MaximumLength(20)
				.WithMessage("PhoneNumber cannot be longer than 20 characters");

		RuleFor(r => r.Address)
			.MaximumLength(255)
				.WithMessage("Address cannot be longer than 255 characters");

		RuleFor(r => r.CitizenshipId)
			.MustAsync(existingEntityCheckerService.IsCorrectCitizenshipIdAsync)
				.WithMessage("Citizenship with this id does not exist");

		RuleFor(r => r.GenderId)
			.MustAsync(existingEntityCheckerService.IsCorrectGenderIdAsync)
				.WithMessage("Gender with this id does not exist");

		RuleFor(r => r.CityId)
			.MustAsync(existingEntityCheckerService.IsCorrectCityIdAsync)
				.WithMessage("City with this id does not exist");
	}
}
