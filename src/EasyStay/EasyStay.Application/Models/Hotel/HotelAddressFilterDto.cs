namespace EasyStay.Application.Models.Hotel;

public class HotelAddressFilterDto {
	public long? Id { get; set; }

	public string? Street { get; set; }

	public string? HouseNumber { get; set; }

	public bool? ByFloor { get; set; }
	public int? Floor { get; set; }

	public bool? ByApartmentNumber { get; set; }
	public string? ApartmentNumber { get; set; }

	public HotelAddressCityFilterDto? City { get; set; }
}
