namespace EasyStay.Domain.Entities;

public class RoomType {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public ICollection<Room> Rooms { get; set; } = null!;
}
