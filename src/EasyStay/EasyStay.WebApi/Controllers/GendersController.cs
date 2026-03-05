using EasyStay.Application.MediatR.Genders.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

public class GendersController : BaseApiController {
	[HttpGet]
	public async Task<IActionResult> GetAll() {
		var items = await Mediator.Send(new GetAllGendersQuery());

		return Ok(items);
	}
}
