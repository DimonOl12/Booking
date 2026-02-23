using EasyStay.Application.MediatR.Languages.Commands.Create;
using EasyStay.Application.MediatR.Languages.Commands.Delete;
using EasyStay.Application.MediatR.Languages.Commands.Update;
using EasyStay.Application.MediatR.Languages.Queries.GetAll;
using EasyStay.Application.MediatR.Languages.Queries.GetDetails;
using EasyStay.Application.MediatR.Languages.Queries.GetPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

public class LanguagesController : BaseApiController {
	[HttpGet]
	public async Task<IActionResult> GetAll() {
		var items = await Mediator.Send(new GetAllLanguagesQuery());
		return Ok(items);
	}

	[HttpGet]
	public async Task<IActionResult> GetPage([FromQuery] GetLanguagesPageQuery command) {
		var page = await Mediator.Send(command);

		return Ok(page);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById([FromRoute] long id) {
		var entity = await Mediator.Send(new GetLanguageDetailsQuery() { Id = id });

		return Ok(entity);
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Create([FromForm] CreateLanguageCommand command) {
		var id = await Mediator.Send(command);

		return Ok(id);
	}

	[HttpPut]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Update([FromForm] UpdateLanguageCommand command) {
		await Mediator.Send(command);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Delete([FromRoute] long id) {
		await Mediator.Send(new DeleteLanguageCommand { Id = id });

		return NoContent();
	}
}
