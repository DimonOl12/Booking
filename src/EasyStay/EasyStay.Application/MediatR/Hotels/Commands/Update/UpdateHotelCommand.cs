using EasyStay.Application.Models.Address;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EasyStay.Application.MediatR.Hotels.Commands.Update;

public class UpdateHotelCommand : IRequest {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public string Description { get; set; } = null!;

	public TimeOnly ArrivalTimeUtcFrom { get; set; }
	public TimeOnly ArrivalTimeUtcTo { get; set; }

	public TimeOnly DepartureTimeUtcFrom { get; set; }
	public TimeOnly DepartureTimeUtcTo { get; set; }

	public bool IsArchived { get; set; }

	public UpdateAddressDto Address { get; set; } = null!;

	public long CategoryId { get; set; }

	public IEnumerable<long>? HotelAmenityIds { get; set; } = null!;

	public IEnumerable<long>? BreakfastIds { get; set; } = null!;

	public IEnumerable<long>? StaffLanguageIds { get; set; } = null!;

	public IEnumerable<IFormFile> Photos { get; set; } = null!;
}
