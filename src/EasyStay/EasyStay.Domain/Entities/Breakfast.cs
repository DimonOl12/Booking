namespace EasyStay.Domain.Entities;

public class Breakfast {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public ICollection<HotelBreakfast> HotelBreakfasts { get; set; } = null!;
}
