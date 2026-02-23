using Bogus;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities;
using EasyStay.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Infrastructure.Services;

public class GeneratedDataSeeder(
	IEasyStayDbContext context,
	IImageService imageService,
	IImageSeeder imageSeeder
) : IGeneratedDataSeeder {

	public async Task SeedAsync(CancellationToken cancellationToken = default) {
		if (!await context.Addresses.AnyAsync(cancellationToken))
			await SeedAddressesAsync(cancellationToken);

		if (!await context.Hotels.AnyAsync(cancellationToken))
			await SeedHotelsAsync(cancellationToken);

		if (!await context.HotelPhotos.AnyAsync(cancellationToken))
			await SeedHotelPhotosAsync(cancellationToken);

		if (!await context.RealtorReviews.AnyAsync(cancellationToken))
			await SeedRealtorReviewsAsync(cancellationToken);

		if (!await context.Rooms.AnyAsync(cancellationToken))
			await SeedRoomsAsync(cancellationToken);

		if (!await context.RoomRentalPeriods.AnyAsync(cancellationToken))
			await SeedRoomRentalPeriodsAsync(cancellationToken);

		if (!await context.RoomRoomAmenities.AnyAsync(cancellationToken))
			await SeedRoomRoomAmenitiesAsync(cancellationToken);

		if (!await context.RoomVariants.AnyAsync(cancellationToken))
			await SeedRoomVariantsAsync(cancellationToken);

		if (!await context.Bookings.AnyAsync(cancellationToken))
			await SeedBookingsAsync(cancellationToken);

		if (!await context.HotelReviews.AnyAsync(cancellationToken))
			await SeedHotelReviewsAsync(cancellationToken);
	}

	private async Task SeedAddressesAsync(CancellationToken cancellationToken) {
		Faker faker = new Faker();

		var cities = await context.Cities.ToArrayAsync(cancellationToken);

		if (cities.Length == 0)
			return;

		for (int i = 0; i < 100; i++) {
			var city = faker.PickRandom(cities);

			int? floor = faker.Random.Int(0, 10);
			if (floor == 0)
				floor = null;

			string? apartmentNumber = null;
			if (floor is not null && faker.Random.Bool(0.5f))
				apartmentNumber = faker.Random.Int(1, 100).ToString();

			var address = new Address() {
				Street = faker.Address.StreetName(),
				HouseNumber = faker.Address.BuildingNumber(),
				CityId = city.Id,
				Floor = floor,
				ApartmentNumber = apartmentNumber
			};

			await context.Addresses.AddAsync(address, cancellationToken);
		}

		await context.SaveChangesAsync(cancellationToken);
	}

	private async Task SeedHotelsAsync(CancellationToken cancellationToken) {
		Faker faker = new Faker();
		Random random = new Random();

		var addressesId = await context.Addresses.Select(c => c.Id).ToArrayAsync(cancellationToken);
		var categoryIds = await context.HotelCategories.Select(hc => hc.Id).ToArrayAsync(cancellationToken);
		var userIds = await context.Realtors.Select(u => u.Id).ToArrayAsync(cancellationToken);
		var hotelAmenityIds = await context.HotelAmenities.Select(ha => ha.Id).ToArrayAsync(cancellationToken);

		foreach (var address in addressesId) {
			int numberOfRooms = random.Next(1, 21);
			double areaPerRoom = Math.Round(5 + (random.NextDouble() * 45), 2);
			double area = Math.Round(numberOfRooms * areaPerRoom, 2);
			var randomHotelAmenityIds = faker.PickRandom(hotelAmenityIds, random.Next(0, hotelAmenityIds.Length));

			Hotel hotel = new() {
				Name = faker.Company.CompanyName(),
				Description = faker.Lorem.Sentences(5),
				ArrivalTimeUtcFrom = GetRandomTimeUtc(faker),
				ArrivalTimeUtcTo = GetRandomTimeUtc(faker),
				DepartureTimeUtcFrom = GetRandomTimeUtc(faker),
				DepartureTimeUtcTo = GetRandomTimeUtc(faker),
				IsArchived = faker.Random.Bool(0.1F),
				AddressId = address,
				HotelCategoryId = faker.PickRandom(categoryIds),
				RealtorId = faker.PickRandom(userIds)
			};

			hotel.HotelHotelAmenities = randomHotelAmenityIds
				.Select(haId => new HotelHotelAmenity {
					Hotel = hotel,
					HotelAmenityId = haId
				})
				.ToArray();

			await context.Hotels.AddAsync(hotel, cancellationToken);
		}

		await context.SaveChangesAsync(cancellationToken);
	}

	private async Task SeedHotelPhotosAsync(CancellationToken cancellationToken) {
		Faker faker = new Faker();

		var hotelIds = await context.Hotels.Select(h => h.Id).ToListAsync(cancellationToken);

		foreach (var hotel in hotelIds) {
			var photoCount = faker.Random.Int(1, 5);

			for (int i = 0; i < photoCount; i++) {
				var imageBytes = await imageSeeder.GetImageBytesAsync(800, 500);

				var hotelPhoto = new HotelPhoto {
					Name = await imageService.SaveImageAsync(imageBytes),
					Priority = i,
					HotelId = hotel
				};

				await context.HotelPhotos.AddAsync(hotelPhoto, cancellationToken);
			}

			await context.SaveChangesAsync(cancellationToken);
		}
	}

	private async Task SeedRealtorReviewsAsync(CancellationToken cancellationToken) {
		Faker faker = new Faker();

		var realtorsIds = await context.Realtors.Select(r => r.Id).ToArrayAsync(cancellationToken);
		var customersIds = await context.Customers.Select(c => c.Id).ToArrayAsync(cancellationToken);

		if (realtorsIds.Length == 0 || customersIds.Length == 0)
			return;

		for (int i = 0; i < 15; i++) {
			var realtorReview = new RealtorReview {
				Description = faker.Lorem.Sentences(3),
				Score = faker.Random.Int(1, 10),
				CreatedAtUtc = DateTime.UtcNow,
				RealtorId = faker.PickRandom(realtorsIds),
				AuthorId = faker.PickRandom(customersIds)
			};

			await context.RealtorReviews.AddAsync(realtorReview, cancellationToken);
		}

		await context.SaveChangesAsync(cancellationToken);
	}

	private async Task SeedRoomsAsync(CancellationToken cancellationToken) {
		var roomTypes = context.RoomTypes.ToArray();

		var hotelsId = await context.Hotels
			.Select(h => h.Id)
			.ToArrayAsync(cancellationToken);

		foreach (var hotelId in hotelsId) {
			var faker = new Faker<Room>()
				.RuleFor(r => r.Name, faker => faker.Commerce.ProductName())
				.RuleFor(r => r.Area, faker => faker.Random.Double(10, 100))
				.RuleFor(r => r.NumberOfRooms, faker => faker.Random.Int(1, 5))
				.RuleFor(r => r.Quantity, faker => faker.Random.Int(1, 10))
				.RuleFor(r => r.HotelId, _ => hotelId)
				.RuleFor(r => r.RoomTypeId, faker => faker.PickRandom(roomTypes).Id);

			var rooms = faker.GenerateBetween(1, 4);
			await context.Rooms.AddRangeAsync(rooms, cancellationToken);
		}

		await context.SaveChangesAsync(cancellationToken);
	}

	private async Task SeedRoomRentalPeriodsAsync(CancellationToken cancellationToken) {
		var rooms = await context.Rooms.ToArrayAsync(cancellationToken);
		var rentalPeriodIds = await context.RentalPeriods
			.Select(rp => rp.Id)
			.ToArrayAsync(cancellationToken);

		foreach (var room in rooms) {
			var faker = new Faker<RoomRentalPeriod>()
				.RuleFor(rrp => rrp.RoomId, _ => room.Id)
				.RuleFor(rrp => rrp.RentalPeriodId, faker => faker.PickRandom(rentalPeriodIds));

			var roomRentalPeriods = faker.GenerateBetween(1, 3).DistinctBy(rrp => rrp.RentalPeriodId);

			await context.RoomRentalPeriods.AddRangeAsync(roomRentalPeriods, cancellationToken);
		}

		await context.SaveChangesAsync(cancellationToken);
	}

	private async Task SeedRoomRoomAmenitiesAsync(CancellationToken cancellationToken) {
		var rooms = await context.Rooms.ToArrayAsync(cancellationToken);
		var roomAmenityIds = await context.RoomAmenities
			.Select(ra => ra.Id)
			.ToArrayAsync(cancellationToken);

		foreach (var room in rooms) {
			var faker = new Faker<RoomRoomAmenity>()
				.RuleFor(rra => rra.RoomId, _ => room.Id)
				.RuleFor(rra => rra.RoomAmenityId, faker => faker.PickRandom(roomAmenityIds));

			var roomRentalPeriods = faker.GenerateBetween(0, 10).DistinctBy(rra => rra.RoomAmenityId);

			await context.RoomRoomAmenities.AddRangeAsync(roomRentalPeriods, cancellationToken);
		}

		await context.SaveChangesAsync(cancellationToken);
	}

	private async Task SeedRoomVariantsAsync(CancellationToken cancellationToken) {
		var roomIds = await context.Rooms
			.Select(r => r.Id)
			.ToArrayAsync(cancellationToken);

		var giFaker = new Faker<GuestInfo>()
			.RuleFor(gi => gi.AdultCount, faker => faker.Random.Int(1, 3))
			.RuleFor(gi => gi.ChildCount, faker => faker.Random.Int(0, 3));

		var biFaker = new Faker<BedInfo>()
			.RuleFor(bi => bi.SingleBedCount, faker => faker.Random.Int(1, 3))
			.RuleFor(bi => bi.DoubleBedCount, faker => faker.Random.Int(0, 2))
			.RuleFor(bi => bi.ExtraBedCount, faker => faker.Random.Int(0, 1))
			.RuleFor(bi => bi.SofaCount, faker => faker.Random.Int(0, 2))
			.RuleFor(bi => bi.KingsizeBedCount, faker => faker.Random.Int(0, 1));

		var rvFaker = new Faker<RoomVariant>()
			.RuleFor(rv => rv.Price, faker => Math.Round(faker.Random.Decimal(0, 5000), 2))
			.RuleFor(
				rv => rv.DiscountPrice,
				(faker, rv) => faker.Random.Bool(0.75F)
					? faker.Random.Decimal(0, rv.Price)
					: null
			)
			.RuleFor(
				rv => rv.GuestInfo,
				(faker, rv) => {
					var guestInfo = giFaker.Generate();
					rv.GuestInfo = guestInfo;
					guestInfo.RoomVariant = rv;
					return guestInfo;
				}
			)
			.RuleFor(
				rv => rv.BedInfo,
				(faker, rv) => {
					var bedInfo = biFaker.Generate();
					rv.BedInfo = bedInfo;
					bedInfo.RoomVariant = rv;
					return bedInfo;
				}
			);

		foreach (var roomId in roomIds) {
			var roomVariants = rvFaker.GenerateBetween(1, 3);

			foreach (var roomVariant in roomVariants)
				roomVariant.RoomId = roomId;

			await context.RoomVariants.AddRangeAsync(roomVariants, cancellationToken);
		}

		await context.SaveChangesAsync(cancellationToken);
	}

	private async Task SeedBookingsAsync(CancellationToken cancellationToken) {
		var faker = new Faker();
		var random = new Random();

		var rooms = await context.Rooms
			.Include(r => r.RoomVariants)
			.ThenInclude(rv => rv.BedInfo)
			.ToArrayAsync(cancellationToken);

		var customers = await context.Customers
			.ToArrayAsync(cancellationToken);

		foreach (var room in rooms) {
			var booking = TryCreateBooking(faker, room, random, customers);

			if (booking is null)
				continue;

			await context.Bookings.AddAsync(booking, cancellationToken);
		}

		await context.SaveChangesAsync(cancellationToken);
	}

	private async Task SeedHotelReviewsAsync(CancellationToken cancellationToken) {
		var bookings = await context.Bookings.ToArrayAsync(cancellationToken);

		var faker = new Faker<HotelReview>()
			.RuleFor(hr => hr.Description, faker => faker.Lorem.Sentences(3))
			.RuleFor(hr => hr.Score, faker => {
				if (faker.Random.Bool(0.2F))
					return null;

				return faker.Random.Int(1, 10);
			})
			.RuleFor(hr => hr.CreatedAtUtc, faker => DateTime.UtcNow.AddHours(faker.Random.Int(-24, 24)));

		var reviews = bookings
			.Select(b => {
				var review = faker.Generate();
				review.BookingId = b.Id;
				return review;
			});

		await context.HotelReviews.AddRangeAsync(reviews, cancellationToken);

		await context.SaveChangesAsync(cancellationToken);
	}



	private static Booking? TryCreateBooking(Faker faker, Room room, Random random, Customer[] customers) {
		var bookingRoomVariants = new List<BookingRoomVariant>();
		int occupiedQuantity = 0;
		decimal totalPrice = 0;

		foreach (var roomVariant in room.RoomVariants) {
			if (random.Next(0, 3) == 0)
				continue;

			var quantity = random.Next(1, 4);
			if (room.Quantity < occupiedQuantity + quantity)
				continue;

			var bookingBedSelection = new BookingBedSelection {
				IsSingleBed = roomVariant.BedInfo.SingleBedCount > 0,
				IsDoubleBed = roomVariant.BedInfo.DoubleBedCount > 0,
				IsExtraBed = roomVariant.BedInfo.ExtraBedCount > 0,
				IsSofa = roomVariant.BedInfo.SofaCount > 0,
				IsKingsizeBed = roomVariant.BedInfo.KingsizeBedCount > 0
			};

			var bookingRoomVariant = new BookingRoomVariant {
				Quantity = quantity,
				RoomVariantId = roomVariant.Id,
				BookingBedSelection = bookingBedSelection
			};

			occupiedQuantity += quantity;
			totalPrice += quantity * roomVariant.Price;

			bookingRoomVariants.Add(bookingRoomVariant);
		}

		if (bookingRoomVariants.Count == 0)
			return null;

		var dateFrom = DateTime.Now.AddDays(random.Next(0, 6));
		var dateTo = dateFrom.AddDays(random.Next(0, 6));

		var daysOccupied = (dateTo - dateFrom).Days + 1;

		var booking = new Booking {
			DateFrom = DateOnly.FromDateTime(dateFrom),
			DateTo = DateOnly.FromDateTime(dateTo),
			PersonalWishes = null,
			EstimatedTimeOfArrivalUtc = GetRandomTimeUtc(faker),
			AmountToPay = totalPrice * daysOccupied,
			CustomerId = faker.PickRandom(customers).Id,
			BookingRoomVariants = bookingRoomVariants
		};

		return booking;
	}

	private static DateTimeOffset GetRandomTimeUtc(Faker faker) {
		var ticks = faker.Date.BetweenTimeOnly(new TimeOnly(2, 0), new TimeOnly(15, 0)).ToTimeSpan().Ticks;

		return new DateTime(ticks, DateTimeKind.Utc);
	}
}
