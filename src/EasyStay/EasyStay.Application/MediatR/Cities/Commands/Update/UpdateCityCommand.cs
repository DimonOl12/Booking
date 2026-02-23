using MediatR;
using Microsoft.AspNetCore.Http;

namespace EasyStay.Application.MediatR.Cities.Commands.Update;

public class UpdateCityCommand : IRequest {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public IFormFile Image { get; set; } = null!;

	public double Longitude { get; set; }

	public double Latitude { get; set; }

	public long CountryId { get; set; }
}
