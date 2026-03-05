using AutoMapper;
using Reservio.Application.Common.Mappings;
using Reservio.Domain.Entities.Identity;

namespace Reservio.Application.MediatR.Chats.Queries.Shared;

public class ChatUserInfoVm : IMapWith<User> {
	public string FullName { get; set; } = null!;

	public string Photo { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<User, ChatUserInfoVm>()
			.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
	}
}

