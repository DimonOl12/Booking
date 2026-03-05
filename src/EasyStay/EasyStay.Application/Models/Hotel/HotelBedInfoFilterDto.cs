namespace EasyStay.Application.Models.Hotel;

public class HotelBedInfoFilterDto {
	public bool? HasSingleBed { get; set; }

	public bool? HasDoubleBed { get; set; }

	public bool? HasExtraBed { get; set; }

	public bool? HasSofa { get; set; }

	public bool? HasKingsizeBed { get; set; }
}
