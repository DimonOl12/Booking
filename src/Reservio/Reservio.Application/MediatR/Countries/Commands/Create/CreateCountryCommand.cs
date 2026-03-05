using MediatR;
using Microsoft.AspNetCore.Http;

namespace Reservio.Application.MediatR.Countries.Commands.Create;

public class CreateCountryCommand : IRequest<long> {
	public string Name { get; set; } = null!;

	public IFormFile Image { get; set; } = null!;
}

