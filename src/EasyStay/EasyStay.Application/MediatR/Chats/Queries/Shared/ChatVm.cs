using AutoMapper;
using EasyStay.Application.Common.Mappings;
using EasyStay.Application.MediatR.Messages.Queries.Shared;
using EasyStay.Domain.Entities;

namespace EasyStay.Application.MediatR.Chats.Queries.Shared;

public class ChatVm : IMapWith<Chat> {
	public long Id { get; set; }

	public long CustomerId { get; set; }
	public ChatUserInfoVm Customer { get; set; } = null!;

	public long RealtorId { get; set; }
	public ChatUserInfoVm Realtor { get; set; } = null!;

	public MessageVm? LastMessage { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<Chat, ChatVm>()
			.ForMember(
				dest => dest.LastMessage,
				opt => opt.MapFrom(
					src => src.Messages
						.OrderByDescending(m => m.CreatedAtUtc)
						.FirstOrDefault()
				)
			);
	}
}
