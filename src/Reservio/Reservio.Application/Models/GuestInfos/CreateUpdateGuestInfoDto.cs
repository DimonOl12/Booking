using AutoMapper;
using Reservio.Application.Common.Mappings;
using Reservio.Application.MediatR.GuestInfos.Commands.Create;
using Reservio.Application.MediatR.GuestInfos.Commands.Update;

namespace Reservio.Application.Models.GuestInfos;

public class CreateUpdateGuestInfoDto : IMapWith<CreateGuestInfoCommand>, IMapWith<UpdateGuestInfoCommand> {
	public int AdultCount { get; set; }

	public int ChildCount { get; set; }



	public void Mapping(Profile profile) {
		profile.CreateMap<CreateUpdateGuestInfoDto, CreateGuestInfoCommand>();
		profile.CreateMap<CreateUpdateGuestInfoDto, UpdateGuestInfoCommand>();
	}
}

