using FluentValidation;

namespace EasyStay.Application.MediatR.HotelCategories.Commands.Create;

public class CreateHotelCategoryValidator : AbstractValidator<CreateHotelCategoryCommand> {
	public CreateHotelCategoryValidator() {
		RuleFor(hc => hc.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null")
			.MaximumLength(255)
				.WithMessage("Name is too long");
	}
}
