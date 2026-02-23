namespace EasyStay.Domain.Entities;

public class HotelHotelAmenity {
	public long HotelId { get; set; }
	public Hotel Hotel { get; set; } = null!;

	public long HotelAmenityId { get; set; }
	public HotelAmenity HotelAmenity { get; set; } = null!;
}
