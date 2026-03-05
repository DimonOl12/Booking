using EasyStay.Application.MediatR.Citizenships.Commands.Create;
using EasyStay.Application.MediatR.Citizenships.Commands.Delete;
using EasyStay.Application.MediatR.Citizenships.Commands.Update;
using EasyStay.Application.MediatR.Citizenships.Queries.GetAll;
using EasyStay.Application.MediatR.Citizenships.Queries.GetDetails;
using EasyStay.Application.MediatR.Citizenships.Queries.GetPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

public class CitizenshipsController : BaseApiController {
	[HttpGet]
	public async Task<IActionResult> GetAll() {
		var items = await Mediator.Send(new GetAllCitizenshipsQuery());
		return Ok(items);
	}

	[HttpGet]
	public async Task<IActionResult> GetPage([FromQuery] GetCitizenshipsPageQuery command) {
		var page = await Mediator.Send(command);

		return Ok(page);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById([FromRoute] long id) {
		var entity = await Mediator.Send(new GetCitizenshipDetailsQuery() { Id = id });

		return Ok(entity);
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Create([FromBody] CreateCitizenshipCommand command) {
		var id = await Mediator.Send(command);

		return Ok(id);
	}

	[HttpPut]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Update([FromBody] UpdateCitizenshipCommand command) {
		await Mediator.Send(command);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Delete([FromRoute] long id) {
		await Mediator.Send(new DeleteCitizenshipCommand { Id = id });

		return NoContent();
	}
}
