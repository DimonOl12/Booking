namespace EasyStay.Domain.Entities;

public class Room {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public double Area { get; set; }

	public int NumberOfRooms { get; set; }

	public int Quantity { get; set; }

	public long HotelId { get; set; }
	public Hotel Hotel { get; set; } = null!;

	public long RoomTypeId { get; set; }
	public RoomType RoomType { get; set; } = null!;

	public ICollection<RoomRentalPeriod> RoomRentalPeriods { get; set; } = null!;

	public ICollection<RoomRoomAmenity> RoomRoomAmenities { get; set; } = null!;

	public ICollection<RoomVariant> RoomVariants { get; set; } = null!;
}
