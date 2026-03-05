using AutoMapper;
using Reservio.Application.Common.Mappings;
using Reservio.Domain.Entities;

namespace Reservio.Application.MediatR.Hotels.Queries.Shared;

public class HotelPhotoVm : IMapWith<HotelPhoto> {
	public string Name { get; set; } = null!;

	public int Priority { get; set; }


	
	public void Mapping(Profile profile) {
		profile.CreateMap<HotelPhoto, HotelPhotoVm>();
	}
}
