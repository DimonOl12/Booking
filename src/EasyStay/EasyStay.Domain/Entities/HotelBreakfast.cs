namespace EasyStay.Domain.Entities;

public class HotelBreakfast {
	public long HotelId { get; set; }
	public Hotel Hotel { get; set; } = null!;

	public long BreakfastId { get; set; }
	public Breakfast Breakfast { get; set; } = null!;
}
