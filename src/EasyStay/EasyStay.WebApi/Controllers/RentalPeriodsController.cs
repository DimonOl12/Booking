using EasyStay.Application.MediatR.RentalPeriods.Commands.Create;
using EasyStay.Application.MediatR.RentalPeriods.Commands.Delete;
using EasyStay.Application.MediatR.RentalPeriods.Commands.Update;
using EasyStay.Application.MediatR.RentalPeriods.Queries.GetAll;
using EasyStay.Application.MediatR.RentalPeriods.Queries.GetDetails;
using EasyStay.Application.MediatR.RentalPeriods.Queries.GetPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

public class RentalPeriodsController : BaseApiController {
	[HttpGet]
	public async Task<IActionResult> GetAll() {
		var items = await Mediator.Send(new GetAllRentalPeriodsQuery());
		return Ok(items);
	}

	[HttpGet]
	public async Task<IActionResult> GetPage([FromQuery] GetRentalPeriodsPageQuery command) {
		var page = await Mediator.Send(command);

		return Ok(page);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById([FromRoute] long id) {
		var entity = await Mediator.Send(new GetRentalPeriodDetailsQuery() { Id = id });

		return Ok(entity);
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Create([FromForm] CreateRentalPeriodCommand command) {
		var id = await Mediator.Send(command);

		return Ok(id);
	}

	[HttpPut]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Update([FromForm] UpdateRentalPeriodCommand command) {
		await Mediator.Send(command);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Delete([FromRoute] long id) {
		await Mediator.Send(new DeleteRentalPeriodCommand { Id = id });

		return NoContent();
	}
}
