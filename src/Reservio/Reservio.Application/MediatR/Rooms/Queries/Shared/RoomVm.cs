using AutoMapper;
using Reservio.Application.Common.Mappings;
using Reservio.Application.MediatR.RentalPeriods.Queries.Shared;
using Reservio.Application.MediatR.RoomAmenities.Queries.Shared;
using Reservio.Application.MediatR.RoomTypes.Queries.Shared;
using Reservio.Application.MediatR.RoomVariants.Queries.Shared;
using Reservio.Domain.Entities;

namespace Reservio.Application.MediatR.Rooms.Queries.Shared;

public class RoomVm : IMapWith<Room> {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public double Area { get; set; }

	public int NumberOfRooms { get; set; }

	public int Quantity { get; set; }

	public long HotelId { get; set; }

	public RoomTypeVm RoomType { get; set; } = null!;

	public IEnumerable<RentalPeriodVm> RentalPeriods { get; set; } = null!;

	public IEnumerable<RoomAmenityVm> Amenities { get; set; } = null!;

	public IEnumerable<RoomVariantVm> Variants { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<Room, RoomVm>()
			.ForMember(
				dest => dest.RentalPeriods,
				opt => opt.MapFrom(
					src => src.RoomRentalPeriods
						.Select(rrp => rrp.RentalPeriod)
						.ToArray()
				)
			)
			.ForMember(
				dest => dest.Amenities,
				opt => opt.MapFrom(
					src => src.RoomRoomAmenities
						.Select(rra => rra.RoomAmenity)
						.ToArray()
				)
			)
			.ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.RoomVariants));
	}
}

