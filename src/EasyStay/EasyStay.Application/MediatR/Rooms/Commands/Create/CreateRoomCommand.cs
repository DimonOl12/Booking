using MediatR;

namespace EasyStay.Application.MediatR.Rooms.Commands.Create;

public class CreateRoomCommand : IRequest<long> {
	public string Name { get; set; } = null!;

	public double Area { get; set; }

	public int NumberOfRooms { get; set; }

	public int Quantity { get; set; }

	public long HotelId { get; set; }

	public long RoomTypeId { get; set; }

	public IEnumerable<long>? RentalPeriodIds { get; set; } = null!;

	public IEnumerable<long>? RoomAmenityIds { get; set; } = null!;
}
