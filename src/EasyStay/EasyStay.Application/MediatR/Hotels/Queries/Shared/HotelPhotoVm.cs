using AutoMapper;
using EasyStay.Application.Common.Mappings;
using EasyStay.Domain.Entities;

namespace EasyStay.Application.MediatR.Hotels.Queries.Shared;

public class HotelPhotoVm : IMapWith<HotelPhoto> {
	public string Name { get; set; } = null!;

	public int Priority { get; set; }


	
	public void Mapping(Profile profile) {
		profile.CreateMap<HotelPhoto, HotelPhotoVm>();
	}
}