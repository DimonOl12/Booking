namespace EasyStay.Domain.Entities.Identity;

public class Customer : User {
	public DateOnly? DateOfBirth { get; set; }

	public string? Address { get; set; }

	public long? CitizenshipId { get; set; }
	public Citizenship? Citizenship { get; set; }

	public long? GenderId { get; set; }
	public Gender? Gender { get; set; }

	public long? CityId { get; set; }
	public City? City { get; set; }

	public ICollection<RealtorReview> RealtorReviews { get; set; } = null!;

	public ICollection<Chat> Chats { get; set; } = null!;

	public ICollection<BankCard> BankCards { get; set; } = null!;

	public ICollection<Booking> Bookings { get; set; } = null!;

	public ICollection<FavoriteHotel> FavoriteHotels { get; set; } = null!;
}
