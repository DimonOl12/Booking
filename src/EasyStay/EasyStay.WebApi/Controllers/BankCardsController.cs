using EasyStay.Application.MediatR.BankCards.Commands.Create;
using EasyStay.Application.MediatR.BankCards.Commands.Delete;
using EasyStay.Application.MediatR.BankCards.Queries.GetAll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyStay.WebApi.Controllers;

public class BankCardsController : BaseApiController {
	[HttpGet]
	[Authorize(Roles = "Customer")]
	public async Task<IActionResult> GetAllAsync() {
		var items = await Mediator.Send(new GetAllBankCardsQuery());
		return Ok(items);
	}

	[HttpPost]
	[Authorize(Roles = "Customer")]
	public async Task<IActionResult> CreateAsync([FromBody] CreateBankCardCommand command) {
		var id = await Mediator.Send(command);
		return Ok(id);
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Customer")]
	public async Task<IActionResult> DeleteAsync([FromRoute] int id) {
		await Mediator.Send(new DeleteBankCardCommand { Id = id });
		return NoContent();
	}
}
