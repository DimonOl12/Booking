using AutoMapper;
using EasyStay.Application.Common.Mappings;
using EasyStay.Domain.Entities.Identity;

namespace EasyStay.Application.MediatR.Accounts.Queries.GetCustomerPage;

public class CustomerItemVm : IMapWith<Customer> {
	public long Id { get; set; }

	public string Email { get; set; } = null!;

	public string UserName { get; set; } = null!;

	public string FirstName { get; set; } = null!;

	public string LastName { get; set; } = null!;

	public string Photo { get; set; } = null!;

	public bool IsLocked { get; set; }



	public void Mapping(Profile profile) {
		profile.CreateMap<Customer, CustomerItemVm>()
			.ForMember(
				dest => dest.IsLocked,
				opt => opt.MapFrom(src => src.LockoutEnd != null && src.LockoutEnd >= DateTimeOffset.UtcNow)
			);
	}
}
