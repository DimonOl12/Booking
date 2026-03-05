using EasyStay.Domain.Entities.Identity;

namespace EasyStay.Domain.Entities;

public class FavoriteHotel {
	public long HotelId;
	public Hotel Hotel = null!;

	public long CustomerId;
	public Customer Customer = null!;
}
