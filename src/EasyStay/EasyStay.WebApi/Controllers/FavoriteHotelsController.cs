using EasyStay.Application.MediatR.FavoriteHotels.Commands.Create;
using EasyStay.Application.MediatR.FavoriteHotels.Commands.Delete;
using EasyStay.Application.MediatR.FavoriteHotels.Queries.IsFavorite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

[Authorize(Roles = "Customer")]
public class FavoriteHotelsController : BaseApiController {
	[HttpGet("{hotelId}")]
	public async Task<IActionResult> IsFavoriteHotel([FromRoute] long hotelId) {
		var entity = await Mediator.Send(new IsFavoriteHotelQuery() { HotelId = hotelId });

		return Ok(entity);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateFavoriteHotelCommand command) {
		await Mediator.Send(command);

		return NoContent();
	}

	[HttpDelete("{hotelId}")]
	public async Task<IActionResult> Delete([FromRoute] long hotelId) {
		await Mediator.Send(new DeleteFavoriteHotelCommand { HotelId = hotelId });

		return NoContent();
	}
}
