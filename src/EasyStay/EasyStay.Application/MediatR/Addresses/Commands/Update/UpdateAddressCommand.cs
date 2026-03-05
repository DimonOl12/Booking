using AutoMapper;
using EasyStay.Application.Common.Mappings;
using EasyStay.Application.Models.Address;
using MediatR;

namespace EasyStay.Application.MediatR.Addresses.Commands.Update;

public class UpdateAddressCommand : IRequest, IMapWith<UpdateAddressDto> {
	public long Id { get; set; }

	public string Street { get; set; } = null!;

	public string HouseNumber { get; set; } = null!;

	public int? Floor { get; set; }

	public string? ApartmentNumber { get; set; }

	public long CityId { get; set; }



	public void Mapping(Profile profile) {
		profile.CreateMap<UpdateAddressDto, UpdateAddressCommand>();
	}
}