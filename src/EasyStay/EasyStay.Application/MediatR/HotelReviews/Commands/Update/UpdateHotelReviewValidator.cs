using FluentValidation;

namespace EasyStay.Application.MediatR.HotelReviews.Commands.Update;

public class UpdateHotelReviewValidator : AbstractValidator<UpdateHotelReviewCommand> {
	public UpdateHotelReviewValidator() {
		RuleFor(r => r.Description)
			.NotEmpty()
				.WithMessage("Description is empty or null.")
			.MaximumLength(4000)
				.WithMessage("Description is too long.");

		RuleFor(r => r.Score)
			.InclusiveBetween(0, 10)
				.WithMessage("Score must be in the range from 0 to 10.");
	}
}
