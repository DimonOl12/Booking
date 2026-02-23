using FluentValidation;

namespace EasyStay.Application.MediatR.HotelAmenities.Commands.Update;

public class UpdateHotelAmenityValidator : AbstractValidator<UpdateHotelAmenityCommand> {
	public UpdateHotelAmenityValidator() {
		RuleFor(ha => ha.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null")
			.MaximumLength(255)
				.WithMessage("Name is too long");
	}
}
