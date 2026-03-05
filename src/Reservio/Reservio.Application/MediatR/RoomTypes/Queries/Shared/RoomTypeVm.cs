using Reservio.Application.Common.Mappings;
using Reservio.Domain.Entities;

namespace Reservio.Application.MediatR.RoomTypes.Queries.Shared;

public class RoomTypeVm : IMapWith<RoomType> {
	public long Id { get; set; }

	public string Name { get; set; } = null!;
}

