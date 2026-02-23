using EasyStay.Application.Common.Mappings;
using EasyStay.Domain.Entities;

namespace EasyStay.Application.MediatR.Bookings.Queries.GetPage;

public class BookingBedSelectionVm : IMapWith<BookingBedSelection> {
	public bool IsSingleBed { get; set; }

	public bool IsDoubleBed { get; set; }

	public bool IsExtraBed { get; set; }

	public bool IsSofa { get; set; }

	public bool IsKingsizeBed { get; set; }
}
