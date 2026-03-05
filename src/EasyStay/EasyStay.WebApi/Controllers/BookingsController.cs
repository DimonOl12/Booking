using EasyStay.Application.MediatR.Bookings.Commands.Create;
using EasyStay.Application.MediatR.Bookings.Queries.GetPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

public class BookingsController : BaseApiController {
	[HttpGet]
	[Authorize(Roles = "Customer")]
	public async Task<IActionResult> GetPageAsync([FromQuery] GetBookingsPageQuery query) {
		return Ok(await Mediator.Send(query));
	}

	[HttpPost]
	[Authorize(Roles = "Customer")]
	public async Task<IActionResult> CreateAsync([FromBody] CreateBookingCommand command) {
		var id = await Mediator.Send(command);

		return Ok(id);
	}
}
