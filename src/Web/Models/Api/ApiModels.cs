namespace Web.Models.Api;

public class JwtResponse
{
    public string Token { get; set; } = "";
}

public class ApiPage<T>
{
    public IEnumerable<T> Data { get; set; } = [];
    public int PagesAvailable { get; set; }
    public int ItemsAvailable { get; set; }
}

public class HotelApiDto
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public double Rating { get; set; }
    public bool IsArchived { get; set; }
    public AddressApiDto Address { get; set; } = new();
    public CategoryApiDto Category { get; set; } = new();
    public IEnumerable<AmenityApiDto> HotelAmenities { get; set; } = [];
    public IEnumerable<PhotoApiDto> Photos { get; set; } = [];
}

public class HotelDetailsApiDto
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal? MinPrice { get; set; }
    public double Rating { get; set; }
    public AddressApiDto Address { get; set; } = new();
    public CategoryApiDto Category { get; set; } = new();
    public IEnumerable<AmenityApiDto> HotelAmenities { get; set; } = [];
    public IEnumerable<SimpleNameApiDto> Breakfasts { get; set; } = [];
    public IEnumerable<SimpleNameApiDto> Languages { get; set; } = [];
    public IEnumerable<RoomApiDto> Rooms { get; set; } = [];
    public IEnumerable<PhotoApiDto> Photos { get; set; } = [];
    public RealtorApiDto Realtor { get; set; } = new();
}

public class AddressApiDto
{
    public string Street { get; set; } = "";
    public string HouseNumber { get; set; } = "";
    public int? Floor { get; set; }
    public CityApiDto City { get; set; } = new();
}

public class CityApiDto
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public CountryApiDto Country { get; set; } = new();
}

public class CountryApiDto
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
}

public class CategoryApiDto
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
}

public class AmenityApiDto
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
}

public class PhotoApiDto
{
    public string Name { get; set; } = "";
    public int Priority { get; set; }
}

public class RealtorApiDto
{
    public long Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Photo { get; set; } = "";
    public string? Description { get; set; }
    public double Rating { get; set; }
}

public class SimpleNameApiDto
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
}

public class RoomApiDto
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public double Area { get; set; }
    public int NumberOfRooms { get; set; }
    public int Quantity { get; set; }
    public SimpleNameApiDto RoomType { get; set; } = new();
    public IEnumerable<AmenityApiDto> Amenities { get; set; } = [];
    public IEnumerable<RoomVariantApiDto> Variants { get; set; } = [];
}

public class RoomVariantApiDto
{
    public long Id { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public long RoomId { get; set; }
    public GuestInfoApiDto GuestInfo { get; set; } = new();
    public BedInfoApiDto BedInfo { get; set; } = new();
}

public class GuestInfoApiDto
{
    public int AdultCount { get; set; }
    public int ChildCount { get; set; }
}

public class BedInfoApiDto
{
    public int SingleBedCount { get; set; }
    public int DoubleBedCount { get; set; }
    public int ExtraBedCount { get; set; }
    public int SofaCount { get; set; }
    public int KingsizeBedCount { get; set; }
}

public class BookingApiDto
{
    public long Id { get; set; }
    public DateOnly DateFrom { get; set; }
    public DateOnly DateTo { get; set; }
    public string? PersonalWishes { get; set; }
    public decimal AmountToPay { get; set; }
    public HotelApiDto Hotel { get; set; } = new();
    public bool HasReview { get; set; }
    public IEnumerable<BookingRoomVariantApiDto> BookingRoomVariants { get; set; } = [];
}

public class BookingRoomVariantApiDto
{
    public long Id { get; set; }
    public RoomVariantApiDto RoomVariant { get; set; } = new();
}

public class CustomerInfoApiDto
{
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string? PhoneNumber { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Address { get; set; }
}
