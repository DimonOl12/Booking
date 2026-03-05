using EasyStay.Application.Interfaces;
using FluentValidation;

namespace EasyStay.Application.MediatR.HotelReviews.Commands.Create;

public class CreateHotelReviewValidator : AbstractValidator<CreateHotelReviewCommand> {
	public CreateHotelReviewValidator(IExistingEntityCheckerService existingEntityCheckerService) {
		RuleFor(hr => hr.Description)
			.NotEmpty()
				.WithMessage("Description is empty or null.")
			.MaximumLength(4000)
				.WithMessage("Description is too long.");

		RuleFor(hr => hr.Score)
			.InclusiveBetween(0, 10)
				.WithMessage("Score must be in the range from 0 to 10.");

		RuleFor(hr => hr.BookingId)
			.NotEqual(0)
				.WithMessage("BookingId cannot be equal to 0.")
			.MustAsync(existingEntityCheckerService.IsCorrectBookingIdOfCurrentUserAsync)
				.WithMessage("Booking with this id does not exist or you do not have permission to access it.")
			.MustAsync(
				async (bookingId, cancellationToken) =>
					!await existingEntityCheckerService.IsCorrectHotelReviewByBookingIdAsync(bookingId, cancellationToken)
			)
				.WithMessage("There are already hotel review for this booking.");
	}
}
