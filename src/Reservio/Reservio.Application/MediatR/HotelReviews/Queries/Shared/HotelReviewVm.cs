using AutoMapper;
using Reservio.Application.Common.Mappings;
using Reservio.Domain.Entities;

namespace Reservio.Application.MediatR.HotelReviews.Queries.Shared;

public class HotelReviewVm : IMapWith<RealtorReview> {
	public long Id { get; set; }

	public string Description { get; set; } = null!;

	public double? Score { get; set; }

	public DateTime CreatedAtUtc { get; set; }

	public DateTime? UpdatedAtUtc { get; set; }

	public long AuthorId { get; set; }
	public AuthorVm Author { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<HotelReview, HotelReviewVm>()
			.ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Booking.Customer));
	}
}

