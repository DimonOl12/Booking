using FluentValidation;

namespace EasyStay.Application.MediatR.Breakfasts.Commands.Update;

public class UpdateBreakfastValidator : AbstractValidator<UpdateBreakfastCommand> {
	public UpdateBreakfastValidator() {
		RuleFor(b => b.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null")
			.MaximumLength(255)
				.WithMessage("Name is too long");
	}
}
