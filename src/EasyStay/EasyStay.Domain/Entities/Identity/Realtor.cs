namespace EasyStay.Domain.Entities.Identity;

public class Realtor : User {
	public string? Description { get; set; }

	public DateOnly? DateOfBirth { get; set; }

	public string? Address { get; set; }

	public long? CitizenshipId { get; set; }
	public Citizenship? Citizenship { get; set; }

	public long? GenderId { get; set; }
	public Gender? Gender { get; set; }

	public long? CityId { get; set; }
	public City? City { get; set; }

	public ICollection<Hotel> Hotels { get; set; } = null!;

	public ICollection<RealtorReview> Reviews { get; set; } = null!;

	public ICollection<Chat> Chats { get; set; } = null!;
}
