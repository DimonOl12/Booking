using Reservio.Domain.Entities.Identity;

namespace Reservio.Domain.Entities;

public class FavoriteHotel {
	public long HotelId;
	public Hotel Hotel = null!;

	public long CustomerId;
	public Customer Customer = null!;
}

