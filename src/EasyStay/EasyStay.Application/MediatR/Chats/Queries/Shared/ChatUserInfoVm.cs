using AutoMapper;
using EasyStay.Application.Common.Mappings;
using EasyStay.Domain.Entities.Identity;

namespace EasyStay.Application.MediatR.Chats.Queries.Shared;

public class ChatUserInfoVm : IMapWith<User> {
	public string FullName { get; set; } = null!;

	public string Photo { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<User, ChatUserInfoVm>()
			.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
	}
}
