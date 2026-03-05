using FluentValidation;

namespace EasyStay.Application.MediatR.Citizenships.Commands.Update;

public class UpdateCitizenshipCommandValidator : AbstractValidator<UpdateCitizenshipCommand> {
	public UpdateCitizenshipCommandValidator() {
		RuleFor(c => c.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null.")
			.MaximumLength(255)
				.WithMessage("Name is too long.");
	}
}
