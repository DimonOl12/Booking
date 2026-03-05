using EasyStay.Application.MediatR.Cities.Commands.Create;
using EasyStay.Application.MediatR.Cities.Commands.Delete;
using EasyStay.Application.MediatR.Cities.Commands.Update;
using EasyStay.Application.MediatR.Cities.Queries.GetAdvertisingPage;
using EasyStay.Application.MediatR.Cities.Queries.GetAll;
using EasyStay.Application.MediatR.Cities.Queries.GetDetails;
using EasyStay.Application.MediatR.Cities.Queries.GetPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

public class CitiesController : BaseApiController {
	[HttpGet]
	public async Task<IActionResult> GetAll() {
		var items = await Mediator.Send(new GetAllCitiesQuery());

		return Ok(items);
	}

	[HttpGet]
	public async Task<IActionResult> GetPage([FromQuery] GetCitiesPageQuery command) {
		var page = await Mediator.Send(command);

		return Ok(page);
	}

	[HttpGet]
	public async Task<IActionResult> GetAdvertisingPageAsync([FromQuery] GetCitiesAdvertisingPageQuery command) =>
		Ok(await Mediator.Send(command));

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById([FromRoute] long id) {
		var entity = await Mediator.Send(new GetCityDetailsQuery() { Id = id });

		return Ok(entity);
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Create([FromForm] CreateCityCommand command) {
		var id = await Mediator.Send(command);

		return Ok(id);
	}

	[HttpPut]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Update([FromForm] UpdateCityCommand command) {
		await Mediator.Send(command);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Delete([FromRoute] long id) {
		await Mediator.Send(new DeleteCityCommand { Id = id });

		return NoContent();
	}
}
