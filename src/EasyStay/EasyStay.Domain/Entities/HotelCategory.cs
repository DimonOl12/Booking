namespace EasyStay.Domain.Entities;

public class HotelCategory {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public ICollection<Hotel> Hotels { get; set; } = null!;
}
