using FluentValidation;

namespace EasyStay.Application.MediatR.RoomTypes.Commands.Update;

public class UpdateRoomTypeCommandValidator : AbstractValidator<UpdateRoomTypeCommand> {
	public UpdateRoomTypeCommandValidator() {
		RuleFor(rt => rt.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null.")
			.MaximumLength(255)
				.WithMessage("Name is too long.");
	}
}
