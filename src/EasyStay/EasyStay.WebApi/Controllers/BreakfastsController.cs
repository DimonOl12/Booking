using EasyStay.Application.MediatR.Breakfasts.Commands.Create;
using EasyStay.Application.MediatR.Breakfasts.Commands.Delete;
using EasyStay.Application.MediatR.Breakfasts.Commands.Update;
using EasyStay.Application.MediatR.Breakfasts.Queries.GetAll;
using EasyStay.Application.MediatR.Breakfasts.Queries.GetDetails;
using EasyStay.Application.MediatR.Breakfasts.Queries.GetPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

public class BreakfastsController : BaseApiController {
	[HttpGet]
	public async Task<IActionResult> GetAll() {
		var items = await Mediator.Send(new GetAllBreakfastsQuery());
		return Ok(items);
	}

	[HttpGet]
	public async Task<IActionResult> GetPage([FromQuery] GetBreakfastsPageQuery command) {
		var page = await Mediator.Send(command);

		return Ok(page);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById([FromRoute] long id) {
		var entity = await Mediator.Send(new GetBreakfastDetailsQuery() { Id = id });

		return Ok(entity);
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Create([FromBody] CreateBreakfastCommand command) {
		var id = await Mediator.Send(command);

		return Ok(id);
	}

	[HttpPut]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Update([FromBody] UpdateBreakfastCommand command) {
		await Mediator.Send(command);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Delete([FromRoute] long id) {
		await Mediator.Send(new DeleteBreakfastCommand { Id = id });

		return NoContent();
	}
}
