using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Chats.Queries.Shared;
using EasyStay.Application.MediatR.Messages.Queries.Shared;
using EasyStay.Domain.Entities;
using EasyStay.Domain.Constants;
using EasyStay.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EasyStay.WebApi.Hubs;

[Authorize(Roles = "Customer,Realtor")]
public class ChatHub(
	IEasyStayDbContext context,
	UserManager<User> userManager,
	IMapper mapper
) : Hub {
	private static class Methods {
		public const string CreateChat = "CreateChat";
		public const string DeleteChat = "DeleteChat";
		public const string ReceiveMessage = "ReceiveMessage";
	}

	public async Task SendMessage(long receiverId, string message) {
		var sender = await GetUserAsync();
		var receiver = await ((await userManager.IsInRoleAsync(sender, Roles.Customer))
			? GetUserOfTypeAsync<Realtor>(receiverId)
			: GetUserOfTypeAsync<Customer>(receiverId));

		if (sender.Id == receiver.Id)
			throw new HubException("Invalid receiverId.");

		long customerId = sender.Id;
		long realtorId = receiver.Id;
		if (!await userManager.IsInRoleAsync(sender, Roles.Customer))
			(customerId, realtorId) = (realtorId, customerId);

		var chat = await context.Chats
			.FirstOrDefaultAsync(c => c.CustomerId == customerId && c.RealtorId == realtorId);

		var isNewChat = chat is null;

		if (chat is null) {
			chat = new Chat {
				CustomerId = customerId,
				RealtorId = realtorId,
				Messages = []
			};

			await context.Chats.AddAsync(chat);
			await context.SaveChangesAsync(CancellationToken.None);
		}

		var messageEntity = new Message {
			Text = message,
			AuthorId = sender.Id,
			CreatedAtUtc = DateTime.UtcNow,
			ChatId = chat!.Id
		};

		await context.Messages.AddAsync(messageEntity);
		await context.SaveChangesAsync(CancellationToken.None);

		if (isNewChat) {
			var chatVm = await context.Chats
				.ProjectTo<ChatVm>(mapper.ConfigurationProvider)
				.FirstAsync(c => c.Id == chat.Id);

			await Clients
				.Users(sender.Id.ToString(), receiver.Id.ToString())
				.SendAsync(Methods.CreateChat, chatVm);
		}

		await Clients
			.Users(sender.Id.ToString(), receiver.Id.ToString())
			.SendAsync(Methods.ReceiveMessage, chat.Id, mapper.Map<MessageVm>(messageEntity));
	}

	public async Task DeleteChat(long chatId) {
		var userId = Convert.ToInt64(UserId);

		var chat = await context.Chats
			.Where(c => c.CustomerId == userId || c.RealtorId == userId)
			.FirstOrDefaultAsync(c => c.Id == chatId)
			?? throw new HubException("Chat not found.");

		context.Chats.Remove(chat);
		await context.SaveChangesAsync(CancellationToken.None);

		await Clients
			.Users(chat.CustomerId.ToString(), chat.RealtorId.ToString())
			.SendAsync(Methods.DeleteChat, chatId);
	}

	private string UserId => Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)
		?? throw new HubException("Unauthorized user.");

	private async Task<User> GetUserAsync() => await userManager.FindByIdAsync(UserId)
		?? throw new HubException("User not found.");

	private async Task<User> GetUserOfTypeAsync<T>(long id) where T : User =>
		await userManager.Users
			.OfType<T>()
			.FirstOrDefaultAsync(u => u.Id == id)
			?? throw new HubException("User not found.");
}
