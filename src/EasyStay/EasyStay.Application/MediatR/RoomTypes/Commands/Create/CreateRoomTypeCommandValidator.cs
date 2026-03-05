using FluentValidation;

namespace EasyStay.Application.MediatR.RoomTypes.Commands.Create;

public class CreateRoomTypeCommandValidator : AbstractValidator<CreateRoomTypeCommand> {
	public CreateRoomTypeCommandValidator() {
		RuleFor(rt => rt.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null.")
			.MaximumLength(255)
				.WithMessage("Name is too long.");
	}
}
