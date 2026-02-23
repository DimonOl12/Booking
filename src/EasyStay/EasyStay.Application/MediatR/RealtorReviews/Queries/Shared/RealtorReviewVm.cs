using AutoMapper;
using EasyStay.Application.Common.Mappings;
using EasyStay.Domain.Entities;

namespace EasyStay.Application.MediatR.RealtorReviews.Queries.Shared;

public class RealtorReviewVm : IMapWith<RealtorReview> {
	public long Id { get; set; }

	public string Description { get; set; } = null!;

	public double? Score { get; set; }

	public DateTime CreatedAtUtc { get; set; }

	public DateTime? UpdatedAtUtc { get; set; }

	public long AuthorId { get; set; }
	public AuthorVm Author { get; set; } = null!;

	public long RealtorId { get; set; }
	public RealtorVm Realtor { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<RealtorReview, RealtorReviewVm>()
			.ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
			.ForMember(dest => dest.Realtor, opt => opt.MapFrom(src => src.Realtor));
	}
}
