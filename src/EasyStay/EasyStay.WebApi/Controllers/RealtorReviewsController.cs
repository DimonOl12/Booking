using EasyStay.Application.MediatR.RealtorReviews.Commands.Create;
using EasyStay.Application.MediatR.RealtorReviews.Commands.Delete;
using EasyStay.Application.MediatR.RealtorReviews.Commands.Update;
using EasyStay.Application.MediatR.RealtorReviews.Queries.GetDetails;
using EasyStay.Application.MediatR.RealtorReviews.Queries.GetPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

public class RealtorReviewsController : BaseApiController {
	[HttpGet]
	public async Task<IActionResult> GetPage([FromQuery] GetRealtorReviewsPageQuery command) {
		var page = await Mediator.Send(command);

		return Ok(page);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById([FromRoute] long id) {
		var entity = await Mediator.Send(new GetRealtorReviewDetalisQuery() { Id = id });

		return Ok(entity);
	}

	[HttpPost]
	[Authorize(Roles = "Customer")]
	public async Task<IActionResult> Create([FromBody] CreateRealtorReviewCommand command) {
		var id = await Mediator.Send(command);

		return Ok(id);
	}

	[HttpPut]
	[Authorize(Roles = "Customer")]
	public async Task<IActionResult> Update([FromBody] UpdateRealtorReviewCommand command) {
		await Mediator.Send(command);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Customer")]
	public async Task<IActionResult> Delete([FromRoute] long id) {
		await Mediator.Send(new DeleteRealtorReviewCommand { Id = id });

		return NoContent();
	}
}
