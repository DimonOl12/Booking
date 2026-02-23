using FluentValidation;

namespace EasyStay.Application.MediatR.HotelAmenities.Commands.Create;

public class CreateHotelAmenityValidator : AbstractValidator<CreateHotelAmenityCommand> {
	public CreateHotelAmenityValidator() {
		RuleFor(ha => ha.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null")
			.MaximumLength(255)
				.WithMessage("Name is too long");
	}
}
