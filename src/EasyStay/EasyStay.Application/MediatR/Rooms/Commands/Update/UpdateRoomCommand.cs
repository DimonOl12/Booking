using MediatR;

namespace EasyStay.Application.MediatR.Rooms.Commands.Update;

public class UpdateRoomCommand : IRequest {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public double Area { get; set; }

	public int NumberOfRooms { get; set; }

	public int Quantity { get; set; }

	public long RoomTypeId { get; set; }

	public IEnumerable<long>? RentalPeriodIds { get; set; } = null!;

	public IEnumerable<long>? RoomAmenityIds { get; set; } = null!;
}
