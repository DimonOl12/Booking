using Reservio.Application.Interfaces;
using FluentValidation;

namespace Reservio.Application.MediatR.Countries.Commands.Create;

public class CreateCountryValidator : AbstractValidator<CreateCountryCommand> {
	public CreateCountryValidator(IImageValidator imageValidator) {
		RuleFor(c => c.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null")
			.MaximumLength(255)
				.WithMessage("Name is too long");

		RuleFor(c => c.Image)
			.NotNull()
				.WithMessage("Image is not selected")
			.MustAsync(imageValidator.IsValidImageAsync)
				.WithMessage("Image is not valid");
	}
}

