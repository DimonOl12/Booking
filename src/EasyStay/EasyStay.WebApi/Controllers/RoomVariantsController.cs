using EasyStay.Application.MediatR.RoomVariants.Commands.Create;
using EasyStay.Application.MediatR.RoomVariants.Commands.Delete;
using EasyStay.Application.MediatR.RoomVariants.Commands.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

public class RoomVariantsController : BaseApiController {
	[HttpPost]
	[Authorize(Roles = "Realtor")]
	public async Task<IActionResult> Create([FromBody] CreateRoomVariantCommand command) {
		var id = await Mediator.Send(command);
		return Ok(id);
	}

	[HttpPut]
	[Authorize(Roles = "Realtor")]
	public async Task<IActionResult> Update([FromBody] UpdateRoomVariantCommand command) {
		await Mediator.Send(command);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Realtor")]
	public async Task<IActionResult> Delete(long id) {
		await Mediator.Send(new DeleteRoomVariantCommand { Id = id });

		return NoContent();
	}
}
