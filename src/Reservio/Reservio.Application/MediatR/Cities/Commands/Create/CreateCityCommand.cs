using MediatR;
using Microsoft.AspNetCore.Http;

namespace Reservio.Application.MediatR.Cities.Commands.Create;

public class CreateCityCommand : IRequest<long> {
	public string Name { get; set; } = null!;

	public IFormFile Image { get; set; } = null!;

	public double Longitude { get; set; }

	public double Latitude { get; set; }

	public long CountryId { get; set; }
}

