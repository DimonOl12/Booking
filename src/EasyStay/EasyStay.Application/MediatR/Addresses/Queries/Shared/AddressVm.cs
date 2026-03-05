using AutoMapper;
using EasyStay.Application.Common.Mappings;
using EasyStay.Application.MediatR.Cities.Queries.Shared;
using EasyStay.Domain.Entities;

namespace EasyStay.Application.MediatR.Addresses.Queries.Shared;

public class AddressVm : IMapWith<Address> {
	public long Id { get; set; }

	public string Street { get; set; } = null!;

	public string HouseNumber { get; set; } = null!;

	public int? Floor { get; set; }

	public string? ApartmentNumber { get; set; }

	public CityVm City { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<Address, AddressVm>();
	}
}