using EasyStay.Application.MediatR.Hotels.Commands.Create;
using EasyStay.Application.MediatR.Hotels.Commands.Delete;
using EasyStay.Application.MediatR.Hotels.Commands.SetArchiveStatus;
using EasyStay.Application.MediatR.Hotels.Commands.Update;
using EasyStay.Application.MediatR.Hotels.Queries.GetAll;
using EasyStay.Application.MediatR.Hotels.Queries.GetDetails;
using EasyStay.Application.MediatR.Hotels.Queries.GetMaxPrice;
using EasyStay.Application.MediatR.Hotels.Queries.GetPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

public class HotelsController : BaseApiController {
	[HttpGet]
	public async Task<IActionResult> GetAll() {
		var items = await Mediator.Send(new GetAllHotelsQuery());

		return Ok(items);
	}

	[HttpGet]
	public async Task<IActionResult> GetPage([FromQuery] GetHotelsPageQuery command) {
		var page = await Mediator.Send(command);

		return Ok(page);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById([FromRoute] long id) {
		var entity = await Mediator.Send(new GetHotelDetailsQuery() { Id = id });

		return Ok(entity);
	}

	[HttpPost]
	[Authorize(Roles = "Realtor")]
	public async Task<IActionResult> Create([FromForm] CreateHotelCommand command) {
		var id = await Mediator.Send(command);

		return Ok(id);
	}

	[HttpPut]
	[Authorize(Roles = "Realtor")]
	public async Task<IActionResult> Update([FromForm] UpdateHotelCommand command) {
		await Mediator.Send(command);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Realtor")]
	public async Task<IActionResult> Delete([FromRoute] long id) {
		await Mediator.Send(new DeleteHotelCommand { Id = id });

		return NoContent();
	}

	[HttpPatch]
	[Authorize(Roles = "Realtor")]
	public async Task<IActionResult> SetArchiveStatus([FromBody] SetArchiveStatusHotelCommand command) {
		await Mediator.Send(command);

		return NoContent();
	}

	[HttpGet]
	public async Task<IActionResult> GetMaxPrice() {
		var maxPrice = await Mediator.Send(new GetMaxPriceCommand());

		return Ok(maxPrice);
	}
}
