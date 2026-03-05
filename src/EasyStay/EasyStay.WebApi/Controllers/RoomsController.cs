using EasyStay.Application.MediatR.Rooms.Commands.Create;
using EasyStay.Application.MediatR.Rooms.Commands.Delete;
using EasyStay.Application.MediatR.Rooms.Commands.Update;
using EasyStay.Application.MediatR.Rooms.Queries.GetAll;
using EasyStay.Application.MediatR.Rooms.Queries.GetDetails;
using EasyStay.Application.MediatR.Rooms.Queries.GetDetailsByRoomVariantId;
using EasyStay.Application.MediatR.Rooms.Queries.GetPage;
using EasyStay.Application.MediatR.Rooms.Queries.GetRoomVariantsFreeQuantity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

[Route("api/[controller]/[action]")]
public class RoomsController : BaseApiController {
	[HttpGet]
	public async Task<IActionResult> GetAll() {
		var items = await Mediator.Send(new GetAllRoomsQuery());

		return Ok(items);
	}

	[HttpGet]
	public async Task<IActionResult> GetPage([FromQuery] GetRoomsPageQuery command) {
		var page = await Mediator.Send(command);

		return Ok(page);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById([FromRoute] long id) {
		var entity = await Mediator.Send(new GetRoomDetailsQuery() { Id = id });

		return Ok(entity);
	}

	[HttpGet("{roomVariantId}")]
	public async Task<IActionResult> GetByRoomVariantId([FromRoute] long roomVariantId) {
		var entity = await Mediator.Send(new GetDetailsByRoomVariantIdQuery() { RoomVariantId = roomVariantId });

		return Ok(entity);
	}

	[HttpGet]
	public async Task<IActionResult> GetRoomVariantsFreeQuantityAsync([FromQuery] GetRoomVariantsFreeQuantityQuery command) {
		var quantity = await Mediator.Send(command);

		return Ok(quantity);
	}

	[HttpPost]
	[Authorize(Roles = "Realtor")]
	public async Task<IActionResult> Create([FromBody] CreateRoomCommand command) {
		var id = await Mediator.Send(command);

		return Ok(id);
	}

	[HttpPut]
	[Authorize(Roles = "Realtor")]
	public async Task<IActionResult> Update([FromBody] UpdateRoomCommand command) {
		await Mediator.Send(command);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Realtor")]
	public async Task<IActionResult> Delete([FromRoute] long id) {
		await Mediator.Send(new DeleteRoomCommand { Id = id });

		return NoContent();
	}
}
