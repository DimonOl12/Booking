using FluentValidation;

namespace EasyStay.Application.MediatR.Breakfasts.Commands.Create;

public class CreateBreakfastValidator : AbstractValidator<CreateBreakfastCommand> {
	public CreateBreakfastValidator() {
		RuleFor(b => b.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null")
			.MaximumLength(255)
				.WithMessage("Name is too long");
	}
}
