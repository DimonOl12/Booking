using AutoMapper;
using Reservio.Application.Common.Mappings;
using Reservio.Domain.Entities.Identity;

namespace Reservio.Application.MediatR.Accounts.Queries.GetCustomersInformation;

public class CustomersInformationVm : IMapWith<Customer> {
	public string FullName { get; set; } = null!;

	public string Email { get; set; } = null!;

	public string? PhoneNumber { get; set; } = null!;

	public DateOnly? DateOfBirth { get; set; }

	public string? Address { get; set; } = null!;

	public CustomersInformationCitizenshipVm? Citizenship { get; set; } = null!;

	public CustomersInformationGenderVm? Gender { get; set; } = null!;

	public CustomersInformationCountryVm? Country { get; set; } = null!;

	public CustomersInformationCityVm? City { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<Customer, CustomersInformationVm>()
			.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
			.ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City!.Country));
	}
}

