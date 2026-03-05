using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.Bookings.Queries.GetPage;

public class GetBookingsPageQuery : PaginationFilterDto, IRequest<PageVm<BookingVm>> {
	public string? OrderBy { get; set; }
}
