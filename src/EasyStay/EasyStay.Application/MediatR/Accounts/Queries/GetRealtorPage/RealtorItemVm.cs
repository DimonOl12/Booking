using AutoMapper;
using EasyStay.Application.Common.Mappings;
using EasyStay.Domain.Entities.Identity;

namespace EasyStay.Application.MediatR.Accounts.Queries.GetRealtorPage;

public class RealtorItemVm : IMapWith<Realtor> {
	public long Id { get; set; }

	public string Email { get; set; } = null!;

	public string UserName { get; set; } = null!;

	public string FirstName { get; set; } = null!;

	public string LastName { get; set; } = null!;

	public string Photo { get; set; } = null!;

	public bool IsLocked { get; set; }



	public void Mapping(Profile profile) {
		profile.CreateMap<Realtor, RealtorItemVm>()
			.ForMember(
				dest => dest.IsLocked,
				opt => opt.MapFrom(src => src.LockoutEnd != null && src.LockoutEnd >= DateTimeOffset.UtcNow)
			);
	}
}
