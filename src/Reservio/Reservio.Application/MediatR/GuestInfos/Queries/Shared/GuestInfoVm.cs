using Reservio.Application.Common.Mappings;
using Reservio.Domain.Entities;

namespace Reservio.Application.MediatR.GuestInfos.Queries.Shared;

public class GuestInfoVm : IMapWith<GuestInfo> {
	public int AdultCount { get; set; }

	public int ChildCount { get; set; }
}

