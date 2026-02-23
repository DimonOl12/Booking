using AutoMapper;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Rooms.Queries.GetPage;
using EasyStay.Application.MediatR.Rooms.Queries.Shared;
using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.WebApi.Services.PaginationServices;

public class RoomPaginationService(
	IEasyStayDbContext context,
	IMapper mapper
) : BasePaginationService<Room, RoomVm, GetRoomsPageQuery>(mapper) {

	protected override IQueryable<Room> GetQuery() => context.Rooms.AsNoTracking().OrderBy(r => r.Id);

	protected override IQueryable<Room> FilterQueryBeforeProjectTo(IQueryable<Room> query, GetRoomsPageQuery filter) {
		if (filter.Name is not null)
			query = query.Where(r => r.Name.ToLower().Contains(filter.Name.ToLower()));

		if (filter.Area is not null)
			query = query.Where(r => r.Area == filter.Area);
		if (filter.MinArea is not null)
			query = query.Where(r => r.Area >= filter.MinArea);
		if (filter.MaxArea is not null)
			query = query.Where(r => r.Area <= filter.MaxArea);

		if (filter.NumberOfRooms is not null)
			query = query.Where(r => r.NumberOfRooms == filter.NumberOfRooms);
		if (filter.MinNumberOfRooms is not null)
			query = query.Where(r => r.NumberOfRooms >= filter.MinNumberOfRooms);
		if (filter.MaxNumberOfRooms is not null)
			query = query.Where(r => r.NumberOfRooms <= filter.MaxNumberOfRooms);

		if (filter.Quantity is not null)
			query = query.Where(r => r.Quantity == filter.Quantity);
		if (filter.MinQuantity is not null)
			query = query.Where(r => r.Quantity >= filter.MinQuantity);
		if (filter.MaxQuantity is not null)
			query = query.Where(r => r.Quantity <= filter.MaxQuantity);

		if (filter.FreePeriod is not null) {
			var period = filter.FreePeriod;

			query = query.Where(
				r => r.Quantity > r.RoomVariants
					.SelectMany(rv => rv.BookingRoomVariants)
					.Where(brv => (period.From <= brv.Booking.DateTo) && (period.To >= brv.Booking.DateFrom))
					.Sum(brv => brv.Quantity)
			);
		}

		if (filter.HotelId is not null)
			query = query.Where(r => r.HotelId == filter.HotelId);

		if (filter.RoomTypeId is not null)
			query = query.Where(r => r.RoomTypeId == filter.RoomTypeId);

		if (filter.AllRentalPeriodIds is not null)
			query = query.Where(
				r => filter.AllRentalPeriodIds.All(
					rpId => r.RoomRentalPeriods.Any(rrp => rrp.RentalPeriodId == rpId)
				)
			);
		if (filter.AnyRentalPeriodIds is not null)
			query = query.Where(
				r => filter.AnyRentalPeriodIds.Any(
					rpId => r.RoomRentalPeriods.Any(rrp => rrp.RentalPeriodId == rpId)
				)
			);

		if (filter.AllAmenityIds is not null)
			query = query.Where(
				r => filter.AllAmenityIds.All(
					amenityId => r.RoomRoomAmenities.Any(ra => ra.RoomAmenityId == amenityId)
				)
			);
		if (filter.AnyAmenityIds is not null)
			query = query.Where(
				r => filter.AnyAmenityIds.Any(
					amenityId => r.RoomRoomAmenities.Any(ra => ra.RoomAmenityId == amenityId)
				)
			);

		return query;
	}
}
