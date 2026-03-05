using AutoMapper;
using Reservio.Application.Common.Mappings;
using Reservio.Domain.Entities;

namespace Reservio.Application.MediatR.HotelAmenities.Queries.Shared;

public class HotelAmenityVm : IMapWith<HotelAmenity> {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public string Image { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<HotelAmenity, HotelAmenityVm>();
	}
}

