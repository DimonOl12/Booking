using EasyStay.Application.Interfaces;
using FluentValidation;

namespace EasyStay.Application.MediatR.Languages.Commands.Create;

public class CreateLanguageValidator : AbstractValidator<CreateLanguageCommand> {
	public CreateLanguageValidator(IExistingEntityCheckerService existingEntityCheckerService) {
		RuleFor(l => l.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null")
			.MaximumLength(255)
				.WithMessage("Name is too long")
			.MustAsync(
				async (name, cancellationToken) =>
					!await existingEntityCheckerService.IsCorrectLanguageNameAsync(name, cancellationToken)
			)
				.WithMessage("Language already exists");
	}
}
