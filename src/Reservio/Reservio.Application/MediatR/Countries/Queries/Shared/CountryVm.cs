using AutoMapper;
using Reservio.Application.Common.Mappings;
using Reservio.Domain.Entities;

namespace Reservio.Application.MediatR.Countries.Queries.Shared;

public class CountryVm : IMapWith<Country> {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public string Image { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<Country, CountryVm>();
	}
}

