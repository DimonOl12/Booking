using EasyStay.Application.MediatR.Messages.Queries.GetAll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

public class MessagesController : BaseApiController {
	[HttpGet]
	[Authorize(Roles = "Customer,Realtor")]
	public async Task<IActionResult> GetAllAsync([FromQuery] GetAllMessagesQuery query) {
		var items = await Mediator.Send(query);
		return Ok(items);
	}
}
