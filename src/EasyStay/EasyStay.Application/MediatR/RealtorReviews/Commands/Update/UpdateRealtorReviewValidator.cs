using FluentValidation;

namespace EasyStay.Application.MediatR.RealtorReviews.Commands.Update;

public class UpdateRealtorReviewValidator : AbstractValidator<UpdateRealtorReviewCommand> {
	public UpdateRealtorReviewValidator() {
		RuleFor(r => r.Description)
			.NotEmpty()
				.WithMessage("Description is empty or null")
			.MaximumLength(4000)
				.WithMessage("Description is too long");

		RuleFor(r => r.Score)
			.InclusiveBetween(0, 10)
				.WithMessage("Score must be in the range from 0 to 10");
	}
}
