using EasyStay.Application.MediatR.HotelAmenities.Commands.Create;
using EasyStay.Application.MediatR.HotelAmenities.Commands.Delete;
using EasyStay.Application.MediatR.HotelAmenities.Commands.Update;
using EasyStay.Application.MediatR.HotelAmenities.Queries.GetAll;
using EasyStay.Application.MediatR.HotelAmenities.Queries.GetDetails;
using EasyStay.Application.MediatR.HotelAmenities.Queries.GetPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

public class HotelAmenitiesController : BaseApiController {
	[HttpGet]
	public async Task<IActionResult> GetAll() {
		var items = await Mediator.Send(new GetAllHotelAmenitiesQuery());
		return Ok(items);
	}

	[HttpGet]
	public async Task<IActionResult> GetPage([FromQuery] GetHotelAmenitiesPageQuery command) {
		var page = await Mediator.Send(command);

		return Ok(page);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById([FromRoute] long id) {
		var entity = await Mediator.Send(new GetHotelAmenityDetailsQuery() { Id = id });

		return Ok(entity);
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Create([FromForm] CreateHotelAmenityCommand command) {
		var id = await Mediator.Send(command);

		return Ok(id);
	}

	[HttpPut]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Update([FromForm] UpdateHotelAmenityCommand command) {
		await Mediator.Send(command);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Delete([FromRoute] long id) {
		await Mediator.Send(new DeleteHotelAmenityCommand { Id = id });

		return NoContent();
	}
}
