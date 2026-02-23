using EasyStay.Application.MediatR.HotelReviews.Commands.Create;
using EasyStay.Application.MediatR.HotelReviews.Commands.Delete;
using EasyStay.Application.MediatR.HotelReviews.Commands.Update;
using EasyStay.Application.MediatR.HotelReviews.Queries.GetDetails;
using EasyStay.Application.MediatR.HotelReviews.Queries.GetPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

public class HotelReviewsController : BaseApiController {
	[HttpGet]
	public async Task<IActionResult> GetPage([FromQuery] GetHotelReviewsPageQuery command) {
		var page = await Mediator.Send(command);

		return Ok(page);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById([FromRoute] long id) {
		var entity = await Mediator.Send(new GetHotelReviewDetalisQuery() { Id = id });

		return Ok(entity);
	}

	[HttpPost]
	[Authorize(Roles = "Customer")]
	public async Task<IActionResult> Create([FromForm] CreateHotelReviewCommand command) {
		var id = await Mediator.Send(command);

		return Ok(id);
	}

	[HttpPut]
	[Authorize(Roles = "Customer")]
	public async Task<IActionResult> Update([FromForm] UpdateHotelReviewCommand command) {
		await Mediator.Send(command);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Customer")]
	public async Task<IActionResult> Delete([FromRoute] long id) {
		await Mediator.Send(new DeleteHotelReviewCommand { Id = id });

		return NoContent();
	}
}
