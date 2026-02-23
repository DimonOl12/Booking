using EasyStay.Application.MediatR.Chats.Queries.GetAll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

public class ChatsController : BaseApiController {
	[HttpGet]
	[Authorize(Roles = "Customer,Realtor")]
	public async Task<IActionResult> GetAllAsync() {
		var items = await Mediator.Send(new GetAllChatsQuery());
		return Ok(items);
	}
}
