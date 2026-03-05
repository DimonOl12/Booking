using Reservio.Application.Common.Mappings;
using Reservio.Domain.Entities;

namespace Reservio.Application.MediatR.RoomAmenities.Queries.Shared;

public class RoomAmenityVm : IMapWith<RoomAmenity> {
	public long Id { get; set; }

	public string Name { get; set; } = null!;
}

