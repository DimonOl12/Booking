using EasyStay.Application.MediatR.HotelCategories.Commands.Create;
using EasyStay.Application.MediatR.HotelCategories.Commands.Delete;
using EasyStay.Application.MediatR.HotelCategories.Commands.Update;
using EasyStay.Application.MediatR.HotelCategories.Queries.GetAll;
using EasyStay.Application.MediatR.HotelCategories.Queries.GetDetails;
using EasyStay.Application.MediatR.HotelCategories.Queries.GetPage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

public class HotelCategoriesController : BaseApiController {
	[HttpGet]
	public async Task<IActionResult> GetAll() {
		var items = await Mediator.Send(new GetAllHotelCategoriesQuery());

		return Ok(items);
	}

	[HttpGet]
	public async Task<IActionResult> GetPage([FromQuery] GetHotelCategoriesPageQuery command) {
		var page = await Mediator.Send(command);

		return Ok(page);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById([FromRoute] long id) {
		var entity = await Mediator.Send(new GetHotelCategoryDetailsQuery() { Id = id });

		return Ok(entity);
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Create([FromForm] CreateHotelCategoryCommand command) {
		var id = await Mediator.Send(command);

		return Ok(id);
	}

	[HttpPut]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Update([FromForm] UpdateHotelCategoryCommand command) {
		await Mediator.Send(command);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Delete([FromRoute] long id) {
		await Mediator.Send(new DeleteHotelCategoryCommand { Id = id });

		return NoContent();
	}
}
