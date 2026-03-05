using AutoMapper;
using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Hotels.Queries.GetPage;
using EasyStay.Application.MediatR.Hotels.Queries.Shared;
using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.WebApi.Services.PaginationServices;

public class HotelPaginationService(
	IEasyStayDbContext context,
	IMapper mapper,
	ITimeConverter timeConverter,
	ICurrentUserService currentUser
) : BasePaginationService<Hotel, HotelVm, GetHotelsPageQuery>(mapper) {

	protected override IQueryable<Hotel> GetQuery() => context.Hotels.AsNoTracking().AsSplitQuery();

	protected override IQueryable<Hotel> FilterQueryBeforeProjectTo(IQueryable<Hotel> query, GetHotelsPageQuery filter) {
		if (filter.IsRandomItems == true) {
			query = query.OrderBy(h => Guid.NewGuid());
		}
		else {
			query = filter.OrderBy switch {
				null => query.OrderBy(h => h.Id),
				"Category" => query.OrderBy(h => h.HotelCategory.Name),
				"Rating" => query.OrderByDescending(
					h => h.Rooms
						.SelectMany(r => r.RoomVariants)
						.SelectMany(rv => rv.BookingRoomVariants)
						.Select(brv => brv.Booking)
						.Average(b => b.HotelReview!.Score)
						.GetValueOrDefault(0)
				),
				"City" => query.OrderBy(h => h.Address.City.Name),
				"RoomsCount" => query.OrderBy(h => h.Rooms.Min(r => r.NumberOfRooms)),
				_ => throw new BadRequestException("Invalid order by parameter"),
			};
		}

		if (filter.Name is not null)
			query = query.Where(h => h.Name.ToLower().Contains(filter.Name.ToLower()));

		if (filter.Description is not null)
			query = query.Where(h => h.Description.ToLower().Contains(filter.Description.ToLower()));

		if (filter.ArrivalTimeUtcFrom.HasValue) {
			var dateTimeOffset = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(filter.ArrivalTimeUtcFrom.Value);
			query = query.Where(h => h.ArrivalTimeUtcFrom == dateTimeOffset);
		}
		if (filter.MinArrivalTimeUtcFrom.HasValue) {
			var dateTimeOffset = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(filter.MinArrivalTimeUtcFrom.Value);
			query = query.Where(h => h.ArrivalTimeUtcFrom >= dateTimeOffset);
		}
		if (filter.MaxArrivalTimeUtcFrom.HasValue) {
			var dateTimeOffset = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(filter.MaxArrivalTimeUtcFrom.Value);
			query = query.Where(h => h.ArrivalTimeUtcFrom <= dateTimeOffset);
		}

		if (filter.ArrivalTimeUtcTo.HasValue) {
			var dateTimeOffset = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(filter.ArrivalTimeUtcTo.Value);
			query = query.Where(h => h.ArrivalTimeUtcTo == dateTimeOffset);
		}
		if (filter.MinArrivalTimeUtcTo.HasValue) {
			var dateTimeOffset = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(filter.MinArrivalTimeUtcTo.Value);
			query = query.Where(h => h.ArrivalTimeUtcTo >= dateTimeOffset);
		}
		if (filter.MaxArrivalTimeUtcTo.HasValue) {
			var dateTimeOffset = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(filter.MaxArrivalTimeUtcTo.Value);
			query = query.Where(h => h.ArrivalTimeUtcTo <= dateTimeOffset);
		}

		if (filter.DepartureTimeUtcFrom.HasValue) {
			var dateTimeOffset = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(filter.DepartureTimeUtcFrom.Value);
			query = query.Where(h => h.DepartureTimeUtcFrom == dateTimeOffset);
		}
		if (filter.MinDepartureTimeUtcFrom.HasValue) {
			var dateTimeOffset = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(filter.MinDepartureTimeUtcFrom.Value);
			query = query.Where(h => h.DepartureTimeUtcFrom >= dateTimeOffset);
		}
		if (filter.MaxDepartureTimeUtcFrom.HasValue) {
			var dateTimeOffset = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(filter.MaxDepartureTimeUtcFrom.Value);
			query = query.Where(h => h.DepartureTimeUtcFrom <= dateTimeOffset);
		}

		if (filter.DepartureTimeUtcTo.HasValue) {
			var dateTimeOffset = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(filter.DepartureTimeUtcTo.Value);
			query = query.Where(h => h.DepartureTimeUtcTo == dateTimeOffset);
		}
		if (filter.MinDepartureTimeUtcTo.HasValue) {
			var dateTimeOffset = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(filter.MinDepartureTimeUtcTo.Value);
			query = query.Where(h => h.DepartureTimeUtcTo >= dateTimeOffset);
		}
		if (filter.MaxDepartureTimeUtcTo.HasValue) {
			var dateTimeOffset = timeConverter.ToDateTimeOffsetFromUtcTimeOnly(filter.MaxDepartureTimeUtcTo.Value);
			query = query.Where(h => h.DepartureTimeUtcTo <= dateTimeOffset);
		}

		if (filter.MinPrice is not null)
			query = query.Where(
				h => h.Rooms
					.SelectMany(r => r.RoomVariants)
					.Any(rv => (rv.DiscountPrice ?? rv.Price) >= filter.MinPrice.Value)
			);

		if (filter.MaxPrice is not null)
			query = query.Where(
				h => h.Rooms
					.SelectMany(r => r.RoomVariants)
					.Any(rv => (rv.DiscountPrice ?? rv.Price) <= filter.MaxPrice.Value)
			);

		if (filter.MinRating is not null)
			query = query.Where(
				h => h.Rooms
					.SelectMany(r => r.RoomVariants)
					.SelectMany(rv => rv.BookingRoomVariants)
					.Select(brv => brv.Booking)
					.Average(b => b.HotelReview!.Score) >= filter.MinRating
			);

		if (filter.HasAnyRoomVariant == true)
			query = query.Where(h => h.Rooms.SelectMany(r => r.RoomVariants).Any());

		if (filter.MinNumberOfRooms is not null)
			query = query.Where(h => h.Rooms.Any(r => r.NumberOfRooms >= filter.MinNumberOfRooms));

		if (filter.MinAdultGuests is not null)
			query = query.Where(h => h.Rooms.Any(r => r.RoomVariants.Any(rv => rv.GuestInfo.AdultCount >= filter.MinAdultGuests)));

		if (filter.FreePeriod is not null) {
			var period = filter.FreePeriod;

			query = query.Where(
				h => h.Rooms.Sum(r => r.Quantity)
					> h.Rooms
						.SelectMany(r => r.RoomVariants)
						.SelectMany(rv => rv.BookingRoomVariants)
						.Where(brv => (period.From <= brv.Booking.DateTo) && (period.To >= brv.Booking.DateFrom))
						.Sum(brv => brv.Quantity)
			);
		}

		if (filter.IsArchived is not null)
			query = query.Where(h => h.IsArchived == filter.IsArchived);

		if (filter.Address is not null) {
			var address = filter.Address;

			if (address.Id is not null)
				query = query.Where(h => h.AddressId == address.Id);

			if (address.Street is not null)
				query = query.Where(h => h.Address.Street.ToLower().Contains(address.Street));

			if (address.HouseNumber is not null)
				query = query.Where(h => h.Address.HouseNumber.ToLower().Contains(address.HouseNumber));

			if (address.ByFloor == true)
				query = query.Where(h => h.Address.Floor == address.Floor);

			if (address.ByApartmentNumber == true)
				query = query.Where(h => h.Address.ApartmentNumber == address.ApartmentNumber);

			if (address.City is not null) {
				var city = address.City;

				if (city.Id is not null)
					query = query.Where(h => h.Address.CityId == city.Id);

				if (city.Name is not null)
					query = query.Where(h => h.Address.City.Name.ToLower().Contains(city.Name.ToLower()));

				if (city.Longitude is not null)
					query = query.Where(h => h.Address.City.Longitude == city.Longitude);

				if (city.Latitude is not null)
					query = query.Where(h => h.Address.City.Latitude == city.Latitude);

				if (city.MinLongitude is not null)
					query = query.Where(h => h.Address.City.Longitude >= city.MinLongitude);
				if (city.MaxLongitude is not null)
					query = query.Where(h => h.Address.City.Longitude <= city.MaxLongitude);

				if (city.MinLatitude is not null)
					query = query.Where(h => h.Address.City.Latitude >= city.MinLatitude);
				if (city.MaxLatitude is not null)
					query = query.Where(h => h.Address.City.Latitude <= city.MaxLatitude);

				if (city.CountryId is not null)
					query = query.Where(h => h.Address.City.CountryId == city.CountryId);
			}
		}

		if (filter.CategoryId is not null)
			query = query.Where(h => h.HotelCategoryId == filter.CategoryId);

		if (filter.RealtorId is not null)
			query = query.Where(h => h.RealtorId == filter.RealtorId);

		if (filter.HasDiscount == true)
			query = query.Where(
				h => h.Rooms.Any(r => r.RoomVariants.Any(rv => rv.DiscountPrice != null))
			);

		if (filter.OnlyOwn == true) {
			var userId = currentUser.GetUserId()
				?? throw new UnauthorizedException("User is not authorized");

			query = query.Where(h => h.RealtorId == userId);
		}

		if (filter.IsFavorite is not null) {
			var userId = currentUser.GetUserId()
				?? throw new UnauthorizedException("User is not authorized");

			query = query.Where(
				h => h.FavoriteHotels
					.Any(fh => fh.HotelId == h.Id && fh.CustomerId == userId) == filter.IsFavorite
			);
		}

		if (filter.AllHotelAmenityIds is not null)
			query = query.Where(
				h => filter.AllHotelAmenityIds.All(
					haId => h.HotelHotelAmenities.Any(hha => hha.HotelAmenityId == haId)
				)
			);

		if (filter.AnyHotelAmenityIds is not null)
			query = query.Where(
				h => filter.AnyHotelAmenityIds.Any(
					haId => h.HotelHotelAmenities.Any(hha => hha.HotelAmenityId == haId)
				)
			);

		if (filter.AllBreakfastIds is not null)
			query = query.Where(
				h => filter.AllBreakfastIds.All(
					bId => h.HotelBreakfasts.Any(hb => hb.BreakfastId == bId)
				)
			);

		if (filter.AnyBreakfastIds is not null)
			query = query.Where(
				h => filter.AnyBreakfastIds.Any(
					bId => h.HotelBreakfasts.Any(hb => hb.BreakfastId == bId)
				)
			);

		if (filter.AllLanguageIds is not null)
			query = query.Where(
				h => filter.AllLanguageIds.All(
					lId => h.HotelStaffLanguages.Any(hsl => hsl.LanguageId == lId)
				)
			);

		if (filter.AnyLanguageIds is not null)
			query = query.Where(
				h => filter.AnyLanguageIds.Any(
					lId => h.HotelStaffLanguages.Any(hsl => hsl.LanguageId == lId)
				)
			);

		if (filter.AllRoomAmenityIds is not null)
			query = query.Where(
				h => h.Rooms.Any(
					r => filter.AllRoomAmenityIds.All(
						raId => r.RoomRoomAmenities.Any(rra => rra.RoomAmenityId == raId)
					)
				)
			);

		if (filter.AnyRoomAmenityIds is not null)
			query = query.Where(
				h => h.Rooms.Any(
					r => filter.AnyRoomAmenityIds.Any(
						raId => r.RoomRoomAmenities.Any(rra => rra.RoomAmenityId == raId)
					)
				)
			);

		if (filter.AllowedRealtorGenders is not null)
			query = query.Where(
				h => h.Realtor.GenderId.HasValue && filter.AllowedRealtorGenders.Contains(h.Realtor.GenderId.Value)
			);

		if (filter.BedInfo is not null) {
			var bedInfo = filter.BedInfo;

			if (bedInfo.HasSingleBed is not null)
				query = query.Where(h => h.Rooms.Any(r => r.RoomVariants.Any(rv => rv.BedInfo.SingleBedCount > 0)));

			if (bedInfo.HasDoubleBed is not null)
				query = query.Where(h => h.Rooms.Any(r => r.RoomVariants.Any(rv => rv.BedInfo.DoubleBedCount > 0)));

			if (bedInfo.HasExtraBed is not null)
				query = query.Where(h => h.Rooms.Any(r => r.RoomVariants.Any(rv => rv.BedInfo.ExtraBedCount > 0)));

			if (bedInfo.HasSofa is not null)
				query = query.Where(h => h.Rooms.Any(r => r.RoomVariants.Any(rv => rv.BedInfo.SofaCount > 0)));

			if (bedInfo.HasKingsizeBed is not null)
				query = query.Where(h => h.Rooms.Any(r => r.RoomVariants.Any(rv => rv.BedInfo.KingsizeBedCount > 0)));
		}

		return query;
	}
}
