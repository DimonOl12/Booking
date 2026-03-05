using EasyStay.Application.Interfaces;
using FluentValidation;

namespace EasyStay.Application.MediatR.FavoriteHotels.Commands.Create;

public class CreateFavoriteHotelCommandValidator : AbstractValidator<CreateFavoriteHotelCommand> {
	public CreateFavoriteHotelCommandValidator(IExistingEntityCheckerService existingEntityCheckerService) {
		RuleFor(fh => fh.HotelId)
			.MustAsync(existingEntityCheckerService.IsCorrectHotelIdAsync)
				.WithMessage("Hotel does not exist")
			.MustAsync(
				async (fhId, cancellationToken) =>
					!await existingEntityCheckerService.IsCorrectFavoriteHotelIdOfCurrentUserAsync(fhId, cancellationToken)
			)
				.WithMessage("Hotel is already favorite");
	}
}
