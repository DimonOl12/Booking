using AutoMapper;
using EasyStay.Application.Common.Mappings;
using EasyStay.Domain.Entities.Identity;

namespace EasyStay.Application.MediatR.Accounts.Queries.GetRealtorsInformation;

public class RealtorsInformationVm : IMapWith<Realtor> {
	public string FullName { get; set; } = null!;

	public string Email { get; set; } = null!;

	public string? Description { get; set; } = null!;

	public string? PhoneNumber { get; set; } = null!;

	public DateOnly? DateOfBirth { get; set; }

	public string? Address { get; set; } = null!;

	public RealtorsInformationCitizenshipVm? Citizenship { get; set; } = null!;

	public RealtorsInformationGenderVm? Gender { get; set; } = null!;

	public RealtorsInformationCountryVm? Country { get; set; } = null!;

	public RealtorsInformationCityVm? City { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<Realtor, RealtorsInformationVm>()
			.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
			.ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City!.Country));
	}
}
