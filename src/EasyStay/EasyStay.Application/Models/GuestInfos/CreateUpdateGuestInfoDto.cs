using AutoMapper;
using EasyStay.Application.Common.Mappings;
using EasyStay.Application.MediatR.GuestInfos.Commands.Create;
using EasyStay.Application.MediatR.GuestInfos.Commands.Update;

namespace EasyStay.Application.Models.GuestInfos;

public class CreateUpdateGuestInfoDto : IMapWith<CreateGuestInfoCommand>, IMapWith<UpdateGuestInfoCommand> {
	public int AdultCount { get; set; }

	public int ChildCount { get; set; }



	public void Mapping(Profile profile) {
		profile.CreateMap<CreateUpdateGuestInfoDto, CreateGuestInfoCommand>();
		profile.CreateMap<CreateUpdateGuestInfoDto, UpdateGuestInfoCommand>();
	}
}
