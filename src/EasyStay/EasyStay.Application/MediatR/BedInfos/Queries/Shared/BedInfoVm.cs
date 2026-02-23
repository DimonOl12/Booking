using EasyStay.Application.Common.Mappings;
using EasyStay.Domain.Entities;

namespace EasyStay.Application.MediatR.BedInfos.Queries.Shared;

public class BedInfoVm : IMapWith<BedInfo> {
	public int SingleBedCount { get; set; }

	public int DoubleBedCount { get; set; }

	public int ExtraBedCount { get; set; }

	public int SofaCount { get; set; }

	public int KingsizeBedCount { get; set; }
}
