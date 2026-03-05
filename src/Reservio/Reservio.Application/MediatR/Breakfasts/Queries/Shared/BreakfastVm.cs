using AutoMapper;
using Reservio.Application.Common.Mappings;
using Reservio.Domain.Entities;

namespace Reservio.Application.MediatR.Breakfasts.Queries.Shared;

public class BreakfastVm : IMapWith<Breakfast> {
	public long Id { get; set; }

	public string Name { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<Breakfast, BreakfastVm>();
	}
}

