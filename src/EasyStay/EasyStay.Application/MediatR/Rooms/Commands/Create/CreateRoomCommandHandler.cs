using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using MediatR;

namespace EasyStay.Application.MediatR.Rooms.Commands.Create;

public class CreateRoomCommandHandler(
	IEasyStayDbContext context
) : IRequestHandler<CreateRoomCommand, long> {

	public async Task<long> Handle(CreateRoomCommand request, CancellationToken cancellationToken) {
		var entity = new Room {
			Name = request.Name,
			Area = request.Area,
			NumberOfRooms = request.NumberOfRooms,
			Quantity = request.Quantity,
			HotelId = request.HotelId,
			RoomTypeId = request.RoomTypeId
		};

		entity.RoomRentalPeriods = (request.RentalPeriodIds ?? [])
			.Select(rpId => new RoomRentalPeriod {
				Room = entity,
				RentalPeriodId = rpId
			})
			.ToArray();

		entity.RoomRoomAmenities = (request.RoomAmenityIds ?? [])
			.Select(raId => new RoomRoomAmenity {
				Room = entity,
				RoomAmenityId = raId
			})
			.ToArray();

		await context.Rooms.AddAsync(entity, cancellationToken);

		await context.SaveChangesAsync(cancellationToken);

		return entity.Id;
	}
}
