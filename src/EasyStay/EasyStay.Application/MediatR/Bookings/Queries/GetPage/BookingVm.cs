using AutoMapper;
using EasyStay.Application.Common.Mappings;
using EasyStay.Application.MediatR.Hotels.Queries.Shared;
using EasyStay.Domain.Entities;

namespace EasyStay.Application.MediatR.Bookings.Queries.GetPage;

public class BookingVm : IMapWith<Booking> {
	public long Id { get; set; }

	public DateOnly DateFrom { get; set; }

	public DateOnly DateTo { get; set; }

	public string? PersonalWishes { get; set; }

	public DateTimeOffset EstimatedTimeOfArrivalUtc { get; set; }

	public decimal AmountToPay { get; set; }

	public IEnumerable<BookingRoomVariantVm> BookingRoomVariants { get; set; } = null!;

	public HotelVm Hotel { get; set; } = null!;

	public bool HasReview { get; set; }



	public void Mapping(Profile profile) {
		profile.CreateMap<Booking, BookingVm>()
			.ForMember(
				dest => dest.Hotel,
				opt => opt.MapFrom(
					src => src.BookingRoomVariants.First().RoomVariant.Room.Hotel
				)
			)
			.ForMember(dest => dest.HasReview, opt => opt.MapFrom(src => src.HotelReview != null));
	}
}
