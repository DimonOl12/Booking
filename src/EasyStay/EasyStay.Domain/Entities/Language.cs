namespace EasyStay.Domain.Entities;

public class Language {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public ICollection<HotelStaffLanguage> HotelStaffLanguages { get; set; } = null!;
}
