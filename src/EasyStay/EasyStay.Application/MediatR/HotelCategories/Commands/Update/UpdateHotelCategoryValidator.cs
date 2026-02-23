using FluentValidation;

namespace EasyStay.Application.MediatR.HotelCategories.Commands.Update;

public class UpdateHotelCategoryValidator : AbstractValidator<UpdateHotelCategoryCommand> {
	public UpdateHotelCategoryValidator() {
		RuleFor(hc => hc.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null")
			.MaximumLength(255)
				.WithMessage("Name is too long");
	}
}
