using EasyStay.Application.MediatR.RoomAmenities.Commands.Create;
using EasyStay.Application.MediatR.RoomAmenities.Commands.Delete;
using EasyStay.Application.MediatR.RoomAmenities.Commands.Update;
using EasyStay.Application.MediatR.RoomAmenities.Queries.GetAll;
using EasyStay.Application.MediatR.RoomAmenities.Queries.GetDetails;
using EasyStay.Application.MediatR.RoomAmenities.Queries.GetPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

public class RoomAmenitiesController : BaseApiController {
	[HttpGet]
	public async Task<IActionResult> GetAll() {
		var items = await Mediator.Send(new GetAllRoomAmenitiesQuery());
		return Ok(items);
	}

	[HttpGet]
	public async Task<IActionResult> GetPage([FromQuery] GetRoomAmenitiesPageQuery command) {
		var page = await Mediator.Send(command);

		return Ok(page);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById([FromRoute] long id) {
		var entity = await Mediator.Send(new GetRoomAmenityDetailsQuery() { Id = id });

		return Ok(entity);
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Create([FromBody] CreateRoomAmenityCommand command) {
		var id = await Mediator.Send(command);

		return Ok(id);
	}

	[HttpPut]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Update([FromBody] UpdateRoomAmenityCommand command) {
		await Mediator.Send(command);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Delete([FromRoute] long id) {
		await Mediator.Send(new DeleteRoomAmenityCommand { Id = id });

		return NoContent();
	}
}
