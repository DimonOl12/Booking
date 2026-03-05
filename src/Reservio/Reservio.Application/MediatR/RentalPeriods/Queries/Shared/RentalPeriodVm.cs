using AutoMapper;
using Reservio.Application.Common.Mappings;
using Reservio.Domain.Entities;

namespace Reservio.Application.MediatR.RentalPeriods.Queries.Shared;

public class RentalPeriodVm : IMapWith<RentalPeriod> {
	public long Id { get; set; }

	public string Name { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<RentalPeriod, RentalPeriodVm>();
	}
}

