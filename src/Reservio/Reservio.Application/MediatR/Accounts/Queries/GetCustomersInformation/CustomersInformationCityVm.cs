using AutoMapper;
using Reservio.Application.Common.Mappings;
using Reservio.Domain.Entities;

namespace Reservio.Application.MediatR.Accounts.Queries.GetCustomersInformation;

public class CustomersInformationCityVm : IMapWith<City> {
	public long Id { get; set; }

	public string Name { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<City, CustomersInformationCityVm>();
	}
}

