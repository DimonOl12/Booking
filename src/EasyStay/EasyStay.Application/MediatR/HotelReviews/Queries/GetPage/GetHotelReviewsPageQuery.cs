using EasyStay.Application.MediatR.HotelReviews.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.HotelReviews.Queries.GetPage;

public class GetHotelReviewsPageQuery : PaginationFilterDto, IRequest<PageVm<HotelReviewVm>> {
	public string? Description { get; set; }

	public double? Score { get; set; }
	public double? MinScore { get; set; }
	public double? MaxScore { get; set; }

	public DateTime? CreatedAtUtc { get; set; }
	public DateTime? MinCreatedAtUtc { get; set; }
	public DateTime? MaxCreatedAtUtc { get; set; }

	public DateTime? UpdatedAtUtc { get; set; }
	public DateTime? MinUpdatedAtUtc { get; set; }
	public DateTime? MaxUpdatedAtUtc { get; set; }

	public long? AuthorId { get; set; }

	public long? HotelId { get; set; }
}
