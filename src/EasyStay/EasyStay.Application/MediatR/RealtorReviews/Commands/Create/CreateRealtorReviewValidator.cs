using FluentValidation;

namespace EasyStay.Application.MediatR.RealtorReviews.Commands.Create;

public class CreateRealtorReviewValidator : AbstractValidator<CreateRealtorReviewCommand> {
	public CreateRealtorReviewValidator() {
		RuleFor(r => r.Description)
			.NotEmpty()
				.WithMessage("Description is empty or null")
			.MaximumLength(4000)
				.WithMessage("Description is too long");

		RuleFor(r => r.Score)
			.InclusiveBetween(0, 10)
				.WithMessage("Score must be in the range from 0 to 10");

		RuleFor(r => r.RealtorId)
			.NotEqual(0)
				.WithMessage("RealtorId cannot be equal to 0");
	}
}
