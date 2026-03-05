namespace EasyStay.Domain.Entities;

public class RoomAmenity {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public ICollection<RoomRoomAmenity> RoomRoomAmenities { get; set; } = null!;
}
