using MediatR;
using Microsoft.AspNetCore.Http;

namespace EasyStay.Application.MediatR.Countries.Commands.Update;

public class UpdateCountryCommand : IRequest {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public IFormFile Image { get; set; } = null!;
}
