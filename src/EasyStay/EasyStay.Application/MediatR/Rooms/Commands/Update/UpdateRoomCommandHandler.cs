using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using EasyStay.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Rooms.Commands.Update;

public class UpdateRoomCommandHandler(
	IEasyStayDbContext context,
	ICurrentUserService currentUserService
) : IRequestHandler<UpdateRoomCommand> {

	public async Task Handle(UpdateRoomCommand request, CancellationToken cancellationToken) {
		var entity = await context.Rooms
			.Include(r => r.Hotel)
			.Include(r => r.RoomRentalPeriods)
			.Include(r => r.RoomRoomAmenities)
			.FirstOrDefaultAsync(
				r => r.Id == request.Id && r.Hotel.RealtorId == currentUserService.GetRequiredUserId(),
				cancellationToken
			)
			?? throw new NotFoundException(nameof(Room), request.Id);

		entity.Name = request.Name;
		entity.Area = request.Area;
		entity.NumberOfRooms = request.NumberOfRooms;
		entity.Quantity = request.Quantity;
		entity.RoomTypeId = request.RoomTypeId;

		entity.RoomRentalPeriods.Clear();
		foreach (var rentalPeriodId in request.RentalPeriodIds ?? [])
			entity.RoomRentalPeriods.Add(new RoomRentalPeriod {
				RoomId = entity.Id,
				RentalPeriodId = rentalPeriodId
			});

		entity.RoomRoomAmenities.Clear();
		foreach (var roomAmenityId in request.RoomAmenityIds ?? [])
			entity.RoomRoomAmenities.Add(new RoomRoomAmenity {
				RoomId = entity.Id,
				RoomAmenityId = roomAmenityId
			});

		await context.SaveChangesAsync(cancellationToken);
	}
}
