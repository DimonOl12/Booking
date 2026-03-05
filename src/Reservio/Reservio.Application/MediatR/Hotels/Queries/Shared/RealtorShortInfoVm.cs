using AutoMapper;
using Reservio.Application.Common.Mappings;
using Reservio.Domain.Entities.Identity;

namespace Reservio.Application.MediatR.Hotels.Queries.Shared;

public class RealtorShortInfoVm : IMapWith<Realtor> {
	public long Id { get; set; }

	public string FirstName { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<Realtor, RealtorShortInfoVm>();
	}
}

