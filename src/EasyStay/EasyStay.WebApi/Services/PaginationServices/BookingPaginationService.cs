using AutoMapper;
using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Bookings.Queries.GetPage;
using EasyStay.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.WebApi.Services.PaginationServices;

public class BookingPaginationService(
	IEasyStayDbContext context,
	IMapper mapper,
	ICurrentUserService currentUserService
) : BasePaginationService<Booking, BookingVm, GetBookingsPageQuery>(mapper) {

	protected override IQueryable<Booking> GetQuery() => context.Bookings
		.AsNoTracking()
		.AsSplitQuery()
		.Where(b => b.CustomerId == currentUserService.GetRequiredUserId());

	protected override IQueryable<Booking> FilterQueryBeforeProjectTo(IQueryable<Booking> query, GetBookingsPageQuery filter) {
		query = filter.OrderBy switch {
			null => query.OrderBy(b => b.Id),
			"NewestByStartDate" => query.OrderByDescending(b => b.DateFrom),
			"OldestByStartDate" => query.OrderBy(b => b.DateFrom),
			"NewestByEndDate" => query.OrderByDescending(b => b.DateTo),
			"OldestByEndDate" => query.OrderBy(b => b.DateTo),
			"LowestAmount" => query.OrderBy(b => b.AmountToPay),
			"HighestAmount" => query.OrderByDescending(b => b.AmountToPay),
			_ => throw new BadRequestException("Invalid order by parameter"),
		};

		return query;
	}
}
