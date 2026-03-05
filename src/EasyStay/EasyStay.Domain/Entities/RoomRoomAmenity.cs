namespace EasyStay.Domain.Entities;

public class RoomRoomAmenity {
	public long RoomId { get; set; }
	public Room Room { get; set; } = null!;

	public long RoomAmenityId { get; set; }
	public RoomAmenity RoomAmenity { get; set; } = null!;
}
