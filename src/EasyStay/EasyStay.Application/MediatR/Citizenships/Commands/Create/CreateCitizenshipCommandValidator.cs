using FluentValidation;

namespace EasyStay.Application.MediatR.Citizenships.Commands.Create;

public class CreateCitizenshipCommandValidator : AbstractValidator<CreateCitizenshipCommand> {
	public CreateCitizenshipCommandValidator() {
		RuleFor(c => c.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null.")
			.MaximumLength(255)
				.WithMessage("Name is too long.");
	}
}
