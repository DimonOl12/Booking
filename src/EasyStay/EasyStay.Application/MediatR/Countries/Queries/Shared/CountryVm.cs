using AutoMapper;
using EasyStay.Application.Common.Mappings;
using EasyStay.Domain.Entities;

namespace EasyStay.Application.MediatR.Countries.Queries.Shared;

public class CountryVm : IMapWith<Country> {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public string Image { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<Country, CountryVm>();
	}
}
