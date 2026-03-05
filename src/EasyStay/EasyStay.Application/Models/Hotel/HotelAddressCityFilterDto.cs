namespace EasyStay.Application.Models.Hotel;

public class HotelAddressCityFilterDto {
	public long? Id { get; set; }

	public string? Name { get; set; }

	public double? Longitude { get; set; }
	public double? Latitude { get; set; }

	public double? MinLongitude { get; set; }
	public double? MaxLongitude { get; set; }
	public double? MinLatitude { get; set; }
	public double? MaxLatitude { get; set; }

	public long? CountryId { get; set; }
}