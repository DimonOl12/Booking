using FluentValidation;

namespace EasyStay.Application.MediatR.RoomAmenities.Commands.Create;

public class CreateRoomAmenityCommandValidator : AbstractValidator<CreateRoomAmenityCommand> {
	public CreateRoomAmenityCommandValidator() {
		RuleFor(ra => ra.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null.")
			.MaximumLength(255)
				.WithMessage("Name is too long.");
	}
}
