using AutoMapper;
using EasyStay.Application.Common.Mappings;
using EasyStay.Application.MediatR.Cities.Queries.Shared;
using EasyStay.Application.MediatR.Citizenships.Queries.Shared;
using EasyStay.Application.MediatR.Genders.Queries.Shared;
using EasyStay.Domain.Entities.Identity;

namespace EasyStay.Application.MediatR.Accounts.Queries.GetRealtorDatails;

public class RealtorDatailsVm : IMapWith<Realtor> {
	public long Id { get; set; }

	public string Email { get; set; } = null!;

	public string? PhoneNumber { get; set; }

	public string UserName { get; set; } = null!;

	public string FirstName { get; set; } = null!;

	public string LastName { get; set; } = null!;

	public string Photo { get; set; } = null!;

	public double Rating { get; set; }

	public string? Description { get; set; }

	public DateOnly? DateOfBirth { get; set; }

	public string? Address { get; set; }

	public CitizenshipVm? Citizenship { get; set; }

	public GenderVm? Gender { get; set; }

	public CityVm? City { get; set; }



	public void Mapping(Profile profile) {
		profile.CreateMap<Realtor, RealtorDatailsVm>()
			.ForMember(
				dest => dest.Rating,
				opt => opt.MapFrom(
					src => src.Reviews
						.Select(r => r.Score)
						.Average()
						.GetValueOrDefault(0)
				)
			);
	}
}
