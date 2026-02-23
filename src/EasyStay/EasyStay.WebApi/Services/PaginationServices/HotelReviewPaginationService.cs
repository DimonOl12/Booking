using AutoMapper;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.HotelReviews.Queries.GetPage;
using EasyStay.Application.MediatR.HotelReviews.Queries.Shared;
using EasyStay.Domain.Entities;

namespace EasyStay.WebApi.Services.PaginationServices;

public class HotelReviewPaginationService(
	IEasyStayDbContext context,
	IMapper mapper
) : BasePaginationService<HotelReview, HotelReviewVm, GetHotelReviewsPageQuery>(mapper) {

	protected override IQueryable<HotelReview> GetQuery() => context.HotelReviews.OrderByDescending(r => r.CreatedAtUtc);

	protected override IQueryable<HotelReview> FilterQueryBeforeProjectTo(IQueryable<HotelReview> query, GetHotelReviewsPageQuery filter) {
		if (filter.Description is not null)
			query = query.Where(hr => hr.Description.ToLower().Contains(filter.Description.ToLower()));

		if (filter.Score is not null)
			query = query.Where(hr => hr.Score == filter.Score);
		if (filter.MinScore is not null)
			query = query.Where(hr => hr.Score >= filter.MinScore);
		if (filter.MaxScore is not null)
			query = query.Where(hr => hr.Score <= filter.MaxScore);

		if (filter.CreatedAtUtc is not null)
			query = query.Where(hr => hr.CreatedAtUtc == filter.CreatedAtUtc);
		if (filter.MinCreatedAtUtc is not null)
			query = query.Where(hr => hr.CreatedAtUtc >= filter.MinCreatedAtUtc);
		if (filter.MaxCreatedAtUtc is not null)
			query = query.Where(hr => hr.CreatedAtUtc <= filter.MaxCreatedAtUtc);

		if (filter.UpdatedAtUtc is not null)
			query = query.Where(hr => hr.UpdatedAtUtc == filter.UpdatedAtUtc);
		if (filter.MinUpdatedAtUtc is not null)
			query = query.Where(hr => hr.UpdatedAtUtc >= filter.MinUpdatedAtUtc);
		if (filter.MaxUpdatedAtUtc is not null)
			query = query.Where(hr => hr.UpdatedAtUtc <= filter.MaxUpdatedAtUtc);

		if (filter.AuthorId is not null)
			query = query.Where(hr => hr.Booking.CustomerId == filter.AuthorId);

		if (filter.HotelId is not null)
			query = query.Where(hr => hr.Booking.BookingRoomVariants.Any(rv => rv.RoomVariant.Room.HotelId == filter.HotelId));

		return query;
	}
}
