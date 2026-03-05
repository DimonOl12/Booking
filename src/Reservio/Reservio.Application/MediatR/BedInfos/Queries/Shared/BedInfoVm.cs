using Reservio.Application.Common.Mappings;
using Reservio.Domain.Entities;

namespace Reservio.Application.MediatR.BedInfos.Queries.Shared;

public class BedInfoVm : IMapWith<BedInfo> {
	public int SingleBedCount { get; set; }

	public int DoubleBedCount { get; set; }

	public int ExtraBedCount { get; set; }

	public int SofaCount { get; set; }

	public int KingsizeBedCount { get; set; }
}

