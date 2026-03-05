namespace EasyStay.Application.Models.Address;

public class UpdateAddressDto {
	public string Street { get; set; } = null!;

	public string HouseNumber { get; set; } = null!;

	public int? Floor { get; set; }

	public string? ApartmentNumber { get; set; }

	public long CityId { get; set; }
}
