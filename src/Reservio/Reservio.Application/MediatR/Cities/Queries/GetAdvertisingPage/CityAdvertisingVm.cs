using AutoMapper;
using Reservio.Application.Common.Mappings;
using Reservio.Domain.Entities;

namespace Reservio.Application.MediatR.Cities.Queries.GetAdvertisingPage;

public class CityAdvertisingVm : IMapWith<City> {
	public string Name { get; set; } = null!;

	public string Image { get; set; } = null!;

	public decimal? MinPrice { get; set; }



	public void Mapping(Profile profile) {
		profile.CreateMap<City, CityAdvertisingVm>()
			.ForMember(
				dest => dest.MinPrice,
				opt => opt.MapFrom(
					src => src.Addresses
						.Select(a => a.Hotel)
						.SelectMany(h => h!.Rooms)
						.SelectMany(r => r.RoomVariants)
						.Min(r => (decimal?)(r.DiscountPrice ?? r.Price))
				)
			);
	}
}

