using EasyStay.Application.Common.Mappings;
using EasyStay.Domain.Entities;

namespace EasyStay.Application.MediatR.RoomTypes.Queries.Shared;

public class RoomTypeVm : IMapWith<RoomType> {
	public long Id { get; set; }

	public string Name { get; set; } = null!;
}
