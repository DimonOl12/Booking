using EasyStay.Application.Interfaces;
using FluentValidation;

namespace EasyStay.Application.MediatR.Rooms.Commands.Update;

public class UpdateRoomValidator : AbstractValidator<UpdateRoomCommand> {
	public UpdateRoomValidator(IExistingEntityCheckerService existingEntityCheckerService, ICollectionValidator collectionValidator) {
		RuleFor(r => r.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null.")
			.MaximumLength(255)
				.WithMessage("Name is too long.");

		RuleFor(r => r.Area)
			.GreaterThan(0)
				.WithMessage("Area cannot be negative or equal to 0.");

		RuleFor(r => r.NumberOfRooms)
			.GreaterThan(0)
				.WithMessage("Number of rooms cannot be negative or equal to 0.");

		RuleFor(r => r.Quantity)
			.GreaterThan(0)
				.WithMessage("Quantity of available rooms cannot be negative or equal to 0.");

		RuleFor(r => r.RoomTypeId)
			.MustAsync(existingEntityCheckerService.IsCorrectRoomTypeIdAsync)
				.WithMessage("RoomType with this id does not exist.");

		RuleFor(r => r.RentalPeriodIds)
			.Must(collectionValidator.IsNullPossibleDistinct)
				.WithMessage("RentalPeriodIds cannot contain duplicates.")
			.MustAsync(existingEntityCheckerService.IsCorrectRentalPeriodIdsAsync)
				.WithMessage("Not all RentalPeriods with this id exists.");

		RuleFor(r => r.RoomAmenityIds)
			.Must(collectionValidator.IsNullPossibleDistinct)
				.WithMessage("RoomAmenityIds cannot contain duplicates.")
			.MustAsync(existingEntityCheckerService.IsCorrectRoomAmenityIdsAsync)
				.WithMessage("Not all RoomAmenities with this id exists.");
	}
}
