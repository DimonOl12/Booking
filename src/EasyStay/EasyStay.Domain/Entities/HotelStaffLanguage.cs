namespace EasyStay.Domain.Entities;

public class HotelStaffLanguage {
	public long HotelId { get; set; }
	public Hotel Hotel { get; set; } = null!;

	public long LanguageId { get; set; }
	public Language Language { get; set; } = null!;
}
