using EasyStay.Application.Interfaces;
using FluentValidation;

namespace EasyStay.Application.MediatR.Addresses.Commands.Update;

public class UpdateAddressValidator : AbstractValidator<UpdateAddressCommand> {
	public UpdateAddressValidator(IExistingEntityCheckerService existingEntityCheckerService) {
		RuleFor(a => a.CityId)
			.MustAsync(existingEntityCheckerService.IsCorrectCityIdAsync)
				.WithMessage("City with this id is not exists");

		RuleFor(a => a.HouseNumber)
			.NotEmpty()
				.WithMessage("House number is empty or null")
			.MaximumLength(255)
				.WithMessage("House number is too long");

		RuleFor(a => a.Street)
			.NotEmpty()
				.WithMessage("Street is empty or null")
			.MaximumLength(255)
				.WithMessage("Street is too long");

		RuleFor(a => a.ApartmentNumber)
			.MaximumLength(20)
				.WithMessage("Apartment number is too long");
	}
}