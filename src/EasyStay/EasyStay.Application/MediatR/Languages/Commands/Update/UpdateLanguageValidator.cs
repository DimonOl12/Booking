using EasyStay.Application.Interfaces;
using FluentValidation;

namespace EasyStay.Application.MediatR.Languages.Commands.Update;

public class UpdateLanguageValidator : AbstractValidator<UpdateLanguageCommand> {
	public UpdateLanguageValidator(IExistingEntityCheckerService existingEntityCheckerService) {
		RuleFor(l => l)
			.MustAsync(
				async (language, cancellationToken) =>
					!await existingEntityCheckerService.IsNewLanguageNameAsync(language.Id, language.Name, cancellationToken)
			)
				.WithMessage("Language already exists");

		RuleFor(l => l.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null")
			.MaximumLength(255)
				.WithMessage("Name is too long");
	}
}
