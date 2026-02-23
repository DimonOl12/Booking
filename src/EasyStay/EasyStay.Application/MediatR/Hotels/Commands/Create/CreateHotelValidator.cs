using EasyStay.Application.Interfaces;
using FluentValidation;

namespace EasyStay.Application.MediatR.Hotels.Commands.Create;

public class CreateHotelValidator : AbstractValidator<CreateHotelCommand> {
	public CreateHotelValidator(IImageValidator imageValidator, IExistingEntityCheckerService existingEntityCheckerService, ICollectionValidator collectionValidator) {
		RuleFor(h => h.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null")
			.MaximumLength(255)
				.WithMessage("Name is too long");

		RuleFor(h => h.Description)
			.NotEmpty()
				.WithMessage("Description is empty or null")
			.MaximumLength(4000)
				.WithMessage("Description is too long (4000)");

		RuleFor(h => h.CategoryId)
			.MustAsync(existingEntityCheckerService.IsCorrectHotelCategoryIdAsync)
				.WithMessage("HotelCategory with this id is not exists");

		RuleFor(h => h.HotelAmenityIds)
			.Must(collectionValidator.IsNullPossibleDistinct)
				.WithMessage("HotelAmenityIds cannot contain duplicates")
			.MustAsync(existingEntityCheckerService.IsCorrectHotelAmenityIdsAsync)
				.WithMessage("Not all HotelAmenities with this id exists");

		RuleFor(h => h.BreakfastIds)
			.Must(collectionValidator.IsNullPossibleDistinct)
				.WithMessage("BreakfastIds cannot contain duplicates")
			.MustAsync(existingEntityCheckerService.IsCorrectBreakfastIdsAsync)
				.WithMessage("Not all Breakfasts with this id exists");

		RuleFor(h => h.StaffLanguageIds)
			.Must(collectionValidator.IsNullPossibleDistinct)
				.WithMessage("StaffLanguageIds cannot contain duplicates")
			.MustAsync(existingEntityCheckerService.IsCorrectLanguageIdsAsync)
				.WithMessage("Not all Languages with this id exists");

		RuleFor(h => h.Photos)
			.MustAsync(imageValidator.IsValidImagesAsync)
				.WithMessage("One ore more of photos are invalid");
	}
}
