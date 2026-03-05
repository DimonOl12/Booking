using EasyStay.Domain.Entities;
using EasyStay.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EasyStay.Application.Interfaces;

public interface IEasyStayDbContext {
	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	int SaveChanges();

	Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);

	DbSet<User> Users { get; set; }
	DbSet<Customer> Customers { get; set; }
	DbSet<Realtor> Realtors { get; set; }
	DbSet<Admin> Admins { get; set; }
	DbSet<Role> Roles { get; set; }

	DbSet<Country> Countries { get; set; }
	DbSet<City> Cities { get; set; }
	DbSet<Address> Addresses { get; set; }
	DbSet<Hotel> Hotels { get; set; }
	DbSet<HotelCategory> HotelCategories { get; set; }
	DbSet<HotelAmenity> HotelAmenities { get; set; }
	DbSet<HotelHotelAmenity> HotelHotelAmenities { get; set; }
	DbSet<Breakfast> Breakfasts { get; set; }
	DbSet<HotelBreakfast> HotelBreakfasts { get; set; }
	DbSet<Language> Languages { get; set; }
	DbSet<HotelStaffLanguage> HotelStaffLanguages { get; set; }
	DbSet<HotelPhoto> HotelPhotos { get; set; }
	DbSet<Room> Rooms { get; set; }
	DbSet<RoomType> RoomTypes { get; set; }
	DbSet<RentalPeriod> RentalPeriods { get; set; }
	DbSet<RoomRentalPeriod> RoomRentalPeriods { get; set; }
	DbSet<RoomAmenity> RoomAmenities { get; set; }
	DbSet<RoomRoomAmenity> RoomRoomAmenities { get; set; }
	DbSet<RoomVariant> RoomVariants { get; set; }
	DbSet<GuestInfo> GuestInfos { get; set; }
	DbSet<BedInfo> BedInfos { get; set; }
	DbSet<RealtorReview> RealtorReviews { get; set; }
	DbSet<Chat> Chats { get; set; }
	DbSet<Message> Messages { get; set; }
	DbSet<Citizenship> Citizenships { get; set; }
	DbSet<Gender> Genders { get; set; }
	DbSet<BankCard> BankCards { get; set; }
	DbSet<Booking> Bookings { get; set; }
	DbSet<BookingRoomVariant> BookingRoomVariants { get; set; }
	DbSet<BookingBedSelection> BookingBedSelections { get; set; }
	DbSet<HotelReview> HotelReviews { get; set; }
	DbSet<FavoriteHotel> FavoriteHotels { get; set; }
}
