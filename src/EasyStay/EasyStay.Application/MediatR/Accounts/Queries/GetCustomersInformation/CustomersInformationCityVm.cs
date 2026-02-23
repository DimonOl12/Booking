using AutoMapper;
using EasyStay.Application.Common.Mappings;
using EasyStay.Domain.Entities;

namespace EasyStay.Application.MediatR.Accounts.Queries.GetCustomersInformation;

public class CustomersInformationCityVm : IMapWith<City> {
	public long Id { get; set; }

	public string Name { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<City, CustomersInformationCityVm>();
	}
}
