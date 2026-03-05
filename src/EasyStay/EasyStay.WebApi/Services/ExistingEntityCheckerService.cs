using EasyStay.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.WebApi.Services;

public class ExistingEntityCheckerService(
	IEasyStayDbContext context,
	ICurrentUserService currentUserService
) : IExistingEntityCheckerService {

	public async Task<bool> IsCorrectCountryIdAsync(long id, CancellationToken cancellationToken) =>
		await context.Countries.AnyAsync(c => c.Id == id, cancellationToken);

	public async Task<bool> IsCorrectCityIdAsync(long id, CancellationToken cancellationToken) =>
		await context.Cities.AnyAsync(c => c.Id == id, cancellationToken);

	public async Task<bool> IsCorrectHotelIdAsync(long id, CancellationToken cancellationToken) =>
		await context.Hotels.AnyAsync(h => h.Id == id, cancellationToken);

	public async Task<bool> IsCorrectHotelIdOfCurrentUserAsync(long id, CancellationToken cancellationToken) =>
		await context.Hotels.AnyAsync(h => h.Id == id && h.RealtorId == currentUserService.GetRequiredUserId(),
			cancellationToken);

	public async Task<bool> IsCorrectHotelCategoryIdAsync(long id, CancellationToken cancellationToken) =>
		await context.HotelCategories.AnyAsync(hc => hc.Id == id, cancellationToken);

	public Task<bool> IsCorrectRentalPeriodIdAsync(long id, CancellationToken cancellationToken) =>
		context.RentalPeriods.AsNoTracking().AnyAsync(rp => rp.Id == id, cancellationToken);

	public async Task<bool> IsCorrectRentalPeriodIdsAsync(IEnumerable<long>? ids, CancellationToken cancellationToken) {
		if (ids is null)
			return true;

		var conveniencesFromDb = await context.RentalPeriods
			.Select(rp => rp.Id)
			.Where(id => ids.Contains(id))
			.ToArrayAsync(cancellationToken);

		return ids.All(conveniencesFromDb.Contains);
	}

	public async Task<bool> IsCorrectHotelAmenityIdsAsync(IEnumerable<long>? ids, CancellationToken cancellationToken) {
		if (ids is null)
			return true;

		var hotelAmenitiesFromDb = await context.HotelAmenities
			.Select(ha => ha.Id)
			.Where(id => ids.Contains(id))
			.ToArrayAsync(cancellationToken);

		return ids.All(hotelAmenitiesFromDb.Contains);
	}

	public async Task<bool> IsCorrectBreakfastIdsAsync(IEnumerable<long>? ids, CancellationToken cancellationToken) {
		if (ids is null)
			return true;

		var breakfastsFromDb = await context.Breakfasts
			.Select(b => b.Id)
			.Where(id => ids.Contains(id))
			.ToArrayAsync(cancellationToken);

		return ids.All(breakfastsFromDb.Contains);
	}

	public Task<bool> IsCorrectRoomIdAsync(long id, CancellationToken cancellationToken) =>
		context.Rooms.AnyAsync(r => r.Id == id, cancellationToken);

	public Task<bool> IsCorrectRoomIdOfCurrentUserAsync(long id, CancellationToken cancellationToken) =>
		 context.Rooms
			.Include(r => r.Hotel)
			.AnyAsync(r => r.Id == id && r.Hotel.RealtorId == currentUserService.GetRequiredUserId(), cancellationToken);

	public async Task<bool> IsCorrectLanguageIdsAsync(IEnumerable<long>? ids, CancellationToken cancellationToken) {
		if (ids is null)
			return true;

		var itemsFromDb = await context.Languages
			.Select(l => l.Id)
			.Where(id => ids.Contains(id))
			.ToArrayAsync(cancellationToken);

		return ids.All(itemsFromDb.Contains);
	}

	public Task<bool> IsCorrectLanguageNameAsync(string name, CancellationToken cancellationToken) =>
		context.Languages.AsNoTracking().AnyAsync(l => l.Name == name, cancellationToken);

	public Task<bool> IsNewLanguageNameAsync(long id, string name, CancellationToken cancellationToken) =>
		context.Languages.AsNoTracking().AnyAsync(l => l.Id != id && l.Name == name, cancellationToken);

	public Task<bool> IsCorrectCitizenshipIdAsync(long id, CancellationToken cancellationToken) =>
		context.Citizenships.AsNoTracking().AnyAsync(c => c.Id == id, cancellationToken);

	public Task<bool> IsCorrectGenderIdAsync(long id, CancellationToken cancellationToken) =>
		context.Genders.AsNoTracking().AnyAsync(g => g.Id == id, cancellationToken);

	public Task<bool> IsCorrectRoomTypeIdAsync(long id, CancellationToken cancellationToken) =>
		context.RoomTypes.AnyAsync(rt => rt.Id == id, cancellationToken);

	public async Task<bool> IsCorrectRoomAmenityIdsAsync(IEnumerable<long>? ids, CancellationToken cancellationToken) {
		if (ids is null)
			return true;

		var itemsFromDb = await context.RoomAmenities
			.Select(ra => ra.Id)
			.Where(id => ids.Contains(id))
			.ToArrayAsync(cancellationToken);

		return ids.All(itemsFromDb.Contains);
	}

	public Task<bool> IsCorrectRoomVariantIdAsync(long id, CancellationToken cancellationToken) =>
		context.RoomVariants.AnyAsync(rv => rv.Id == id, cancellationToken);

	public async Task<bool> IsCorrectRoomVariantIdsAsync(IEnumerable<long>? ids, CancellationToken cancellationToken) {
		if (ids is null)
			return true;

		var itemsFromDb = await context.RoomVariants
			.Select(rv => rv.Id)
			.Where(id => ids.Contains(id))
			.ToArrayAsync(cancellationToken);

		return ids.All(itemsFromDb.Contains);
	}

	public Task<bool> IsCorrectRoomVariantIdOfCurrentUserAsync(long id, CancellationToken cancellationToken) =>
		context.RoomVariants.Include(rv => rv.Room).ThenInclude(r => r.Hotel).AnyAsync(
			rv => rv.Id == id && rv.Room.Hotel.RealtorId == currentUserService.GetRequiredUserId(),
			cancellationToken
		);

	public Task<bool> IsCorrectBankCardIdOfCurrentUserAsync(long id, CancellationToken cancellationToken) =>
		context.BankCards.AnyAsync(b => b.Id == id && b.CustomerId == currentUserService.GetRequiredUserId(), cancellationToken);

	public Task<bool> IsCorrectBookingIdOfCurrentUserAsync(long id, CancellationToken cancellationToken) =>
		context.Bookings.AnyAsync(b => b.Id == id && b.CustomerId == currentUserService.GetRequiredUserId(), cancellationToken);

	public Task<bool> IsCorrectHotelReviewByBookingIdAsync(long bookingId, CancellationToken cancellationToken) =>
		context.HotelReviews.AnyAsync(hr => hr.BookingId == bookingId, cancellationToken);

	public Task<bool> IsCorrectFavoriteHotelIdOfCurrentUserAsync(long hotelId, CancellationToken cancellationToken) =>
		context.FavoriteHotels.AnyAsync(fh => fh.HotelId == hotelId && fh.CustomerId == currentUserService.GetRequiredUserId(), cancellationToken);
}
