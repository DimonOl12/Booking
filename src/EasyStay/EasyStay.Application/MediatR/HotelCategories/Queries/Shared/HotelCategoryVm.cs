using AutoMapper;
using EasyStay.Application.Common.Mappings;
using EasyStay.Domain.Entities;

namespace EasyStay.Application.MediatR.HotelCategories.Queries.Shared;

public class HotelCategoryVm : IMapWith<HotelCategory> {
	public long Id { get; set; }

	public string Name { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<HotelCategory, HotelCategoryVm>();
	}
}
