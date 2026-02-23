using EasyStay.Application.Interfaces;
using FluentValidation;

namespace EasyStay.Application.MediatR.Accounts.Commands.SetPhoto;

public class SetPhotoValidator : AbstractValidator<SetPhotoCommand> {
	public SetPhotoValidator(IImageValidator imageValidator) {
		RuleFor(p => p.Photo)
			.NotNull()
				.WithMessage("Image is not selected.")
			.MustAsync(imageValidator.IsValidImageAsync)
				.WithMessage("Image is not valid.");
	}
}
